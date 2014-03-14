using log4net;
using PromoStudio.Common.Enumerations;
using PromoStudio.Common.Models;
using PromoStudio.Data;
using PromoStudio.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VimeoDotNet.Enums;

namespace PromoStudio.CloudStatusService
{
    public class CloudStatusProcessor : ICloudStatusProcessor
    {
        private IDataService _dataService;
        private IStreamingProvider _streamingProvider;
        private ILog _log;

        public CloudStatusProcessor(IDataService dataService,  IStreamingProvider streamingProvider, ILog log)
        {
            _dataService = dataService;
            _streamingProvider = streamingProvider;
            _log = log;
        }

        public async Task Execute() {
            var videos = await RetrieveVideosForProcessing();
            if (videos == null) { return; }

            foreach (var video in videos)
            {
                CheckVideo(video);
            }
        }

        private void CheckVideo(CustomerVideo video)
        {
            if (!video.VimeoVideoId.HasValue)
            {
                _log.Error("Error checking customer video for cloud status check. Missing Vimeo ID, id: " + video.pk_CustomerVideoId);
                return;
            }
            try
            {
                var vimeoVideo = _streamingProvider.GetVideo(video.VimeoVideoId.Value);
                if (vimeoVideo != null && vimeoVideo.VideoStatus == VideoStatusEnum.Available)
                {
                    video.fk_CustomerVideoRenderStatusId = (sbyte) CustomerVideoRenderStatus.Completed;
                    if (vimeoVideo.pictures != null)
                    {
                        var thumb = vimeoVideo.pictures.FirstOrDefault(p => p.PictureType == PictureTypeEnum.Thumbnail);
                        if (thumb != null)
                        {
                            video.VimeoThumbnailUrl = thumb.link;
                        }
                    }

                    // TODO: Check if this is the right link
                    video.VimeoStreamingUrl = vimeoVideo.StreamingVideoSecureLink ?? vimeoVideo.StreamingVideoLink;
                    _dataService.CustomerVideo_Update(video);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error checking customer video for cloud status check, id: " + video.pk_CustomerVideoId, ex);
            }
        }

        private async Task<IList<CustomerVideo>> RetrieveVideosForProcessing()
        {
            try
            {
                var videosToCheck = (await _dataService.CustomerVideo_SelectForCloudStatusCheckAsync())
                    .ToList();
                if (videosToCheck.Count == 0) {
                    _log.Debug("No CustomerVideo objects ready for status check. Sleeping...");
                    return null;
                }

                return videosToCheck;
            }
            catch (Exception ex)
            {
                _log.Error("Error retrieving CustomerVideos from queue for status check.", ex);
                throw;
            }
        }
    }
}
