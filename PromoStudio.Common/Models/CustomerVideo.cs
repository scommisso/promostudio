﻿using PromoStudio.Common.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PromoStudio.Common.Models
{
    public class CustomerVideo
    {
        private List<CustomerVideoItem> _items = new List<CustomerVideoItem>();

        public long pk_CustomerVideoId { get; set; }
        public long fk_CustomerId { get; set; }
        public sbyte fk_CustomerVideoRenderStatusId { get; set; }
        public int fk_StoryboardId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string RenderFailureMessage { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public DateTime? DateCompleted { get; set; }
        public string PreviewFilePath { get; set; }
        public string CompletedFilePath { get; set; }
        public long? VidyardVideoId { get; set; }
        public long? VidyardPlayerId { get; set; }
        public string VidyardPlayerUuid { get; set; }

        public CustomerVideoRenderStatus RenderStatus
        {
            get { return (CustomerVideoRenderStatus)fk_CustomerVideoRenderStatusId; }
            set { fk_CustomerVideoRenderStatusId = (sbyte)value; }
        }

        public Storyboard Storyboard { get; set; }
        public CustomerVideoVoiceOver VoiceOver { get; set; }

        public List<CustomerVideoItem> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public string GetVideoItemsJson(bool isPreview)
        {
            if (Items == null) { return "[]"; }
            return string.Format("[{0}]", string.Join(",",
                Items
                .Where(i =>
                    i.Type == CustomerVideoItemType.StockVideo
                    || (i.Type == CustomerVideoItemType.CustomerTemplateScript))
                .OrderBy(i => i.SortOrder ?? 0)
                .Select(i => i.GetRenderItemJson(isPreview))
                .Where(json => json != null)));
        }

        public string GetAudioItemsJson()
        {
            if (Items == null) { return "[]"; }
            return string.Format("[{0}]", string.Join(",",
                Items
                .Where(i =>
                    i.Type == CustomerVideoItemType.StockAudio
                    || (i.Type == CustomerVideoItemType.CustomerVideoVoiceOver))
                .Select(i => i.GetRenderItemJson(false))
                .Where(json => json != null)));
        }

        public dynamic ToPoco()
        {
            return new
            {
                pk_CustomerVideoId = pk_CustomerVideoId,
                fk_CustomerId = fk_CustomerId,
                fk_CustomerVideoRenderStatusId = fk_CustomerVideoRenderStatusId,
                fk_StoryboardId = fk_StoryboardId,
                Name = Name,
                Description = Description,
                RenderFailureMessage = RenderFailureMessage,
                DateCreated = DateCreated,
                DateUpdated = DateUpdated,
                DateCompleted = DateCompleted,
                PreviewFilePath = PreviewFilePath,
                CompletedFilePath = CompletedFilePath,
                VidyardVideoId = VidyardVideoId,
                VidyardPlayerId = VidyardPlayerId,
                VidyardPlayerUuid = VidyardPlayerUuid
            };
        }
    }
}
