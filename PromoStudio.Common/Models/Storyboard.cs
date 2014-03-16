using System;
using System.Collections.Generic;
using PromoStudio.Common.Enumerations;

namespace PromoStudio.Common.Models
{
    public class Storyboard
    {
        private List<StoryboardItem> _storyboardItems = new List<StoryboardItem>();

        public int pk_StoryboardId { get; set; }
        public sbyte fk_StoryboardStatusId { get; set; }
        public int? fk_OrganizationId { get; set; }
        public int? fk_VerticalId { get; set; }
        public int? fk_AudioScriptTemplateId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long VimeoVideoId { get; set; }
        public string VimeoThumbnailUrl { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        public AudioScriptTemplate AudioScriptTemplate { get; set; }

        public List<StoryboardItem> Items
        {
            get { return _storyboardItems; }
            set { _storyboardItems = value; }
        }

        public StoryboardStatus Status
        {
            get { return (StoryboardStatus) fk_StoryboardStatusId; }
            set { fk_StoryboardStatusId = (sbyte) value; }
        }

        public dynamic ToPoco()
        {
            return new
            {
                pk_StoryboardId,
                fk_StoryboardStatusId,
                fk_OrganizationId,
                fk_VerticalId,
                fk_AudioScriptTemplateId,
                Name,
                Description,
                VimeoVideoId,
                VimeoThumbnailUrl,
                DateCreated,
                DateUpdated
            };
        }
    }
}