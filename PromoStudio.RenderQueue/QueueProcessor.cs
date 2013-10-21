using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using PromoStudio.Common.Enumerations;
using PromoStudio.Common.Extensions;
using PromoStudio.Common.Models;
using PromoStudio.Data;
using PromoStudio.Rendering;
using PromoStudio.RenderQueue.Properties;
using PromoStudio.Storage;

namespace PromoStudio.RenderQueue
{
    public class QueueProcessor : IQueueProcessor
    {
        private IDataService _dataService;
        private IStorageProvider _storageProvider;
        private IStreamingProvider _streamingProvider;
        private ILog _log;

        public QueueProcessor(IDataService dataService, IStorageProvider storageProvider, IStreamingProvider streamingProvider, ILog log)
        {
            _dataService = dataService;
            _storageProvider = storageProvider;
            _streamingProvider = streamingProvider;
            _log = log;
        }

        public async Task Execute() {
            var video = await RetrieveVideoForProcessing();
            if (video == null) { return; }
            video.RenderFailureMessage = null;

            ProcessVideo(video);
            UploadVideo(video);
        }

        private async Task<CustomerVideo> RetrieveVideoForProcessing()
        {
            try
            {
                var videosToProcess = await _dataService.CustomerVideo_SelectForProcessingAsync(Settings.Default.ErrorProcessingDelayInSeconds);
                if (videosToProcess.Count() == 0) {
                    _log.Debug("No CustomerVideo objects ready for processing. Sleeping...");
                    return null;
                }
                var video = videosToProcess.Take(1).FirstOrDefault();
                video = await _dataService.CustomerVideo_SelectWithItemsAsync(video.pk_CustomerVideoId);
                _log.InfoFormat("Retrieved CustomerVideo:{0} for processing. Status: {1}",
                    video.pk_CustomerVideoId, (CustomerVideoRenderStatus)video.fk_CustomerVideoRenderStatusId);
                return video;
            }
            catch (Exception ex)
            {
                _log.Error("Error retrieving CustomerVideo from queue for processing.", ex);
                throw;
            }
        }

        private void UploadVideo(CustomerVideo video)
        {
            bool isPreview = (video.fk_CustomerVideoRenderStatusId < (sbyte)CustomerVideoRenderStatus.PendingVoiceTalent);
            if (!new[] {
                CustomerVideoRenderStatus.CompletedVideoPreview,
                CustomerVideoRenderStatus.UploadingPreview,
                CustomerVideoRenderStatus.CompletedFinalRender,
                CustomerVideoRenderStatus.UploadingFinalRender
            }.Contains((CustomerVideoRenderStatus)video.fk_CustomerVideoRenderStatusId))
            {
                // Not in state to upload
                _log.InfoFormat("CustomerVideo:{0} status {1} is not ready for upload.",
                    video.pk_CustomerVideoId, (CustomerVideoRenderStatus)video.fk_CustomerVideoRenderStatusId);
                return;
            }

            try
            {
                string bucketName = video.fk_CustomerId.ToString();
                string fileName;
                string outputUrl;
                if (isPreview)
                {
                    video.fk_CustomerVideoRenderStatusId = (sbyte)CustomerVideoRenderStatus.UploadingPreview;
                    _dataService.CustomerVideo_Update(video);

                    fileName = string.Format("{0}-preview.mov", video.pk_CustomerVideoId);
                    _log.InfoFormat("CustomerVideo:{0} uploading file \"{1}\" to bucket \"{2}\", file \"{3}\".",
                        video.pk_CustomerVideoId, video.PreviewFilePath, bucketName, fileName);
                    outputUrl = _storageProvider.StoreFile(bucketName, fileName, video.PreviewFilePath);
                    video.PreviewFilePath = outputUrl;

                    string cloudId = _streamingProvider.StoreFile(outputUrl, video.Name, video.Description);
                    video.VidyardId = cloudId;

                    CleanupTempFolder(Path.GetDirectoryName(video.PreviewFilePath));

                    if (video.Items.Any(cvi =>
                        cvi.fk_CustomerVideoItemTypeId == (sbyte) CustomerVideoItemType.CustomerVideoVoiceOver))
                    {
                        // HACK: This must be demo mode, set status to completed as this is the final render
                        video.fk_CustomerVideoRenderStatusId = (sbyte)CustomerVideoRenderStatus.Completed;
                    }
                    else
                    {
                        video.fk_CustomerVideoRenderStatusId = (sbyte) CustomerVideoRenderStatus.PendingVoiceTalent;
                    }
                }
                else
                {
                    video.fk_CustomerVideoRenderStatusId = (sbyte)CustomerVideoRenderStatus.UploadingFinalRender;
                    _dataService.CustomerVideo_Update(video);

                    fileName = string.Format("{0}.mov", video.pk_CustomerVideoId);
                    _log.InfoFormat("CustomerVideo:{0} uploading file \"{1}\" to bucket \"{2}\", file \"{3}\".",
                        video.pk_CustomerVideoId, video.PreviewFilePath, fileName, bucketName);
                    outputUrl = _storageProvider.StoreFile(bucketName, fileName, video.CompletedFilePath);

                    CleanupTempFolder(Path.GetDirectoryName(video.CompletedFilePath));

                    video.CompletedFilePath = outputUrl;
                    video.fk_CustomerVideoRenderStatusId = (sbyte)CustomerVideoRenderStatus.Completed;
                }

                _dataService.CustomerVideo_Update(video);
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("CustomerVideo:{0} failure uploading {1} file \"{2}\".",
                    video.pk_CustomerVideoId, isPreview ? "preview" : "final", isPreview ? video.PreviewFilePath : video.CompletedFilePath), ex);
                video.RenderFailureMessage = string.Format("Failure uploading file \"{0}\", error: {1}",
                    isPreview ? video.PreviewFilePath : video.CompletedFilePath, ex.ToString());
                video.fk_CustomerVideoRenderStatusId = (sbyte) (isPreview
                    ? CustomerVideoRenderStatus.CompletedVideoPreview
                    : CustomerVideoRenderStatus.CompletedFinalRender);
                try
                {
                    _dataService.CustomerVideo_Update(video);
                }
                catch (Exception ex2)
                {
                    _log.Error(string.Format("CustomerVideo:{0} error setting failure status", video.pk_CustomerVideoId), ex2);
                }
            }
        }

        private void ProcessVideo(CustomerVideo video)
        {
            bool isPreview = (video.fk_CustomerVideoRenderStatusId < (sbyte)CustomerVideoRenderStatus.UploadingPreview);
            try
            {
                if (video.fk_CustomerVideoRenderStatusId > (sbyte)CustomerVideoRenderStatus.CompletedFinalRender
                    || (video.fk_CustomerVideoRenderStatusId >= (sbyte)CustomerVideoRenderStatus.UploadingPreview
                    && video.fk_CustomerVideoRenderStatusId < (sbyte)CustomerVideoRenderStatus.CompletedVoiceTalent))
                {
                    return;
                    // Completed, or waiting for upload or voice talent
                }

                video.RenderFailureMessage = null; // clear any prior errors

                // Check if template is required to render
                if ((isPreview && video.fk_CustomerVideoRenderStatusId < (sbyte)CustomerVideoRenderStatus.CompletedTemplatePreview) ||
                    (!isPreview && video.fk_CustomerVideoRenderStatusId < (sbyte)CustomerVideoRenderStatus.CompletedTemplate))
                {
                    ProcessTemplates(video, isPreview);
                }

                // Check if splice is required to render
                if ((isPreview && video.fk_CustomerVideoRenderStatusId < (sbyte)CustomerVideoRenderStatus.CompletedVideoPreview) ||
                    (!isPreview && video.fk_CustomerVideoRenderStatusId < (sbyte)CustomerVideoRenderStatus.CompletedFinalRender))
                {
                    ProcessSplice(video, isPreview);
                }
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("CustomerVideo:{0} failure processing {1} file.",
                    video.pk_CustomerVideoId, isPreview ? "preview" : "final"), ex);
                video.RenderFailureMessage = string.Format("Failure processing {0} file, error: {1}",
                    isPreview ? "preview" : "final", ex.ToString());
                try
                {
                    _dataService.CustomerVideo_Update(video);
                }
                catch (Exception ex2)
                {
                    _log.Error(string.Format("CustomerVideo:{0} error setting failure status", video.pk_CustomerVideoId), ex2);
                }
            }
        }

        private void ProcessTemplates(CustomerVideo video, bool preview)
        {
            video.fk_CustomerVideoRenderStatusId = preview
                ? (sbyte)CustomerVideoRenderStatus.InProgressTemplatePreview
                : (sbyte)CustomerVideoRenderStatus.InProgressTemplate;
            _dataService.CustomerVideo_Update(video);

            var templateScripts = video.Items
                .Where(cvi => cvi.CustomerScript != null)
                .Select(cvi => cvi.CustomerScript)
                .ToArray();
            foreach (var customerTemplateScript in templateScripts)
            {
                RenderTemplateScript(video, customerTemplateScript, preview);
            }

            video.fk_CustomerVideoRenderStatusId = preview
                ? (sbyte)CustomerVideoRenderStatus.CompletedTemplatePreview
                : (sbyte)CustomerVideoRenderStatus.CompletedTemplate;
            video.fk_CustomerVideoRenderStatusId = (sbyte)CustomerVideoRenderStatus.CompletedTemplatePreview;
            _dataService.CustomerVideo_Update(video);
        }

        private void ProcessSplice(CustomerVideo video, bool preview)
        {
            video.fk_CustomerVideoRenderStatusId = preview
                ? (sbyte)CustomerVideoRenderStatus.InProgressVideoPreview
                : (sbyte)CustomerVideoRenderStatus.InProgressFinalRender;
            _dataService.CustomerVideo_Update(video);

            string folder = CreateTempFolder(video.fk_CustomerId, video.pk_CustomerVideoId);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string outPath = Path.Combine(folder, string.Format("{0}_{1}_{2}.mov", video.pk_CustomerVideoId, video.Name, preview ? "preview" : "final").ToSafeFileName());

            if (File.Exists(outPath))
            {
                File.Delete(outPath);
            }

            _log.InfoFormat("CustomerVideo:{0} performing splice for {1:N0} items, outputting file \"{2}\".",
                video.pk_CustomerVideoId, video.Items.Count, outPath);
            var renderer = new SpliceTemplateScript(video, outPath, true);
            renderer.Render();
            if (!File.Exists(outPath))
            {
                throw new ApplicationException("Splice output not found: " + outPath);
            }
            using (var sr = File.OpenRead(outPath))
            {
                if (!sr.CanRead)
                {
                    throw new ArgumentException(string.Format("Splice output \"{0}\" cannot be opened for reading.", outPath));
                }
                if (sr.Length <= 0)
                {
                    throw new ArgumentException(string.Format("Splice output \"{0}\" contains 0 bytes.", outPath));
                }
            }

            if (preview)
            {
                video.PreviewFilePath = outPath;
            }
            else
            {
                video.CompletedFilePath = outPath;
            }

            video.fk_CustomerVideoRenderStatusId = preview
                ? (sbyte)CustomerVideoRenderStatus.CompletedVideoPreview
                : (sbyte)CustomerVideoRenderStatus.CompletedFinalRender;
            _dataService.CustomerVideo_Update(video);
        }

        private string CreateTempFolder(long customerId, long customerVideoId)
        {
            string folder = Path.Combine(
                PromoStudio.Rendering.Properties.Settings.Default.OutputPath,
                string.Format("{0}\\{1}", customerId, customerVideoId));
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            return folder;
        }

        private void CleanupTempFolder(string directoryPath)
        {
            try
            {
                if (Directory.Exists(directoryPath))
                {
                    Directory.Delete(directoryPath, true);
                }
            }
            catch (Exception ex)
            {
                _log.Warn(string.Format("Error cleaning up temporary folder \"{0}\".", directoryPath), ex);
            }
        }

        private void RenderTemplateScript(CustomerVideo video, CustomerTemplateScript customerTemplateScript, bool preview)
        {
            string folder = CreateTempFolder(video.fk_CustomerId, video.pk_CustomerVideoId);

            string outPath = Path.Combine(folder, string.Format("{0}_{1}_{2}_{3}.mov",
                customerTemplateScript.pk_CustomerTemplateScriptId, video.Name,
                customerTemplateScript.Template.Name, preview ? "preview" : "final").ToSafeFileName());

            if (File.Exists(outPath))
            {
                File.Delete(outPath);
            }

            _log.InfoFormat("CustomerTemplateScript:{0} performing {1} render to \"{2}\"",
                customerTemplateScript.pk_CustomerTemplateScriptId, preview ? "preview" : "final", outPath);
            var renderer = new RenderTemplateScript(customerTemplateScript, outPath, preview);
            renderer.Render();
            if (!File.Exists(outPath))
            {
                throw new ApplicationException("Template output not found: " + outPath);
            }
            using (var sr = File.OpenRead(outPath))
            {
                if (!sr.CanRead)
                {
                    throw new ArgumentException(string.Format("Template output \"{0}\" cannot be opened for reading.", outPath));
                }
                if (sr.Length <= 0)
                {
                    throw new ArgumentException(string.Format("Template output \"{0}\" contains 0 bytes.", outPath));
                }
            }
            
            if (preview)
            {
                customerTemplateScript.PreviewFilePath = outPath;
            }
            else
            {
                customerTemplateScript.CompletedFilePath = outPath;
            }

            _dataService.CustomerTemplateScript_Update(customerTemplateScript);
        }
    }
}
