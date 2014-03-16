﻿using System;
using System.Collections.Generic;
using PromoStudio.Common.Enumerations;

namespace PromoStudio.Common.Models
{
    public class TemplateScript
    {
        private List<TemplateScriptItem> _scriptItems = new List<TemplateScriptItem>();

        public long pk_TemplateScriptId { get; set; }
        public sbyte fk_TemplateScriptStatusId { get; set; }
        public int? fk_OrganizationId { get; set; }
        public short fk_StoryboardItemType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProjectFilePath { get; set; }
        public string ThumbnailFilePath { get; set; }
        public string PreviewFilePath { get; set; }
        public string RenderCompName { get; set; }
        public string RenderPreviewCompName { get; set; }
        public double? RenderCompStartTime { get; set; }
        public double? RenderCompEndTime { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        public List<TemplateScriptItem> Items
        {
            get { return _scriptItems; }
            set { _scriptItems = value; }
        }

        public TemplateScriptStatus Status
        {
            get { return (TemplateScriptStatus) fk_TemplateScriptStatusId; }
            set { fk_TemplateScriptStatusId = (sbyte) value; }
        }

        public StoryboardItemType StoryboardType
        {
            get { return (StoryboardItemType) fk_StoryboardItemType; }
            set { fk_StoryboardItemType = (short) value; }
        }

        public dynamic ToPoco()
        {
            return new
            {
                pk_TemplateScriptId,
                fk_TemplateScriptStatusId,
                fk_OrganizationId,
                fk_StoryboardItemType,
                Name,
                Description,
                ProjectFilePath,
                ThumbnailFilePath,
                PreviewFilePath,
                RenderCompName,
                RenderPreviewCompName,
                RenderCompStartTime,
                RenderCompEndTime,
                DateCreated,
                DateUpdated
            };
        }
    }
}