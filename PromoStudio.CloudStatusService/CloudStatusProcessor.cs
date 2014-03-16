using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using PromoStudio.Common.Enumerations;
using PromoStudio.Common.Models;
using PromoStudio.Data;
using PromoStudio.Storage;
using VimeoDotNet.Enums;
using VimeoDotNet.Models;

namespace PromoStudio.CloudStatusService
{
    public class CloudStatusProcessor : ICloudStatusProcessor
    {
        private readonly IDataService _dataService;
        private readonly ILog _log;
        private readonly IStreamingProvider _streamingProvider;

        public CloudStatusProcessor(IDataService dataService, IStreamingProvider streamingProvider, ILog log)
        {
            _dataService = dataService;
            _streamingProvider = streamingProvider;
            _log = log;
        }

        public async Task Execute()
        {
            IList<CustomerVideo> videos = await RetrieveVideosForProcessing();
            if (videos == null)
            {
                return;
            }

            foreach (CustomerVideo video in videos)
            {
                CheckVideo(video);
            }
        }

        private void CheckVideo(CustomerVideo video)
        {
            if (!video.VimeoVideoId.HasValue)
            {
                _log.Error("Error checking customer video for cloud status check. Missing Vimeo ID, id: " +
                           video.pk_CustomerVideoId);
                return;
            }
            try
            {
                Video vimeoVideo = _streamingProvider.GetVideo(video.VimeoVideoId.Value);
                if (vimeoVideo != null && vimeoVideo.VideoStatus == VideoStatusEnum.Available)
                {
                    video.fk_CustomerVideoRenderStatusId = (sbyte) CustomerVideoRenderStatus.Completed;
                    if (vimeoVideo.pictures != null)
                    {
                        Picture thumb =
                            vimeoVideo.pictures.FirstOrDefault(p => p.PictureType == PictureTypeEnum.Thumbnail);
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
                List<CustomerVideo> videosToCheck = (await _dataService.CustomerVideo_SelectForCloudStatusCheckAsync())
                    .ToList();
                if (videosToCheck.Count == 0)
                {
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