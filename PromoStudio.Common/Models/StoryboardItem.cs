using PromoStudio.Common.Enumerations;

namespace PromoStudio.Common.Models
{
    public class StoryboardItem
    {
        public long pk_StoryboardItemId { get; set; }
        public int fk_StoryboardId { get; set; }
        public short fk_StoryboardItemTypeId { get; set; }
        public long? fk_TemplateScriptId { get; set; }
        public string Name { get; set; }
        public sbyte SortOrder { get; set; }

        public StoryboardItemType Type {
            get { return (StoryboardItemType)fk_StoryboardItemTypeId; }
            set { fk_StoryboardItemTypeId = (sbyte)value; }
        }

        public TemplateScript TemplateScript { get; set; }

        public dynamic ToPoco()
        {
            return new
            {
                pk_StoryboardItemId = pk_StoryboardItemId,
                fk_StoryboardId = fk_StoryboardId,
                fk_StoryboardItemTypeId = fk_StoryboardItemTypeId,
                fk_TemplateScriptId = fk_TemplateScriptId,
                Name = Name,
                SortOrder = SortOrder
            };
        }
    }
}
