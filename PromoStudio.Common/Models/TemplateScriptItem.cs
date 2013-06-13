using PromoStudio.Common.Enumerations;
using PromoStudio.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoStudio.Common.Models
{
    public class TemplateScriptItem
    {
        public long pk_TemplateScriptItemId { get; set; }
        public long fk_TemplateScriptId { get; set; }
        public sbyte fk_TemplateScriptItemTypeId { get; set; }
        public short fk_TemplateScriptItemCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ItemWidth { get; set; }
        public int ItemHeight { get; set; }

        public TemplateScriptItemType Type {
            get { return (TemplateScriptItemType)fk_TemplateScriptItemTypeId; }
            set { fk_TemplateScriptItemTypeId = (sbyte)value; }
        }

        public TemplateScriptItemCategory Category
        {
            get { return (TemplateScriptItemCategory)fk_TemplateScriptItemCategoryId; }
            set { fk_TemplateScriptItemCategoryId = (short)value; }
        }

        public string GetSwapItemJson(string swapValue)
        {
            var sb = new StringBuilder();
            sb.Append("{ type: \"");
            if (Type == TemplateScriptItemType.Text)
            {
                sb.AppendFormat("Text\", layer: \"{0}\", text: \"{1}\" }}", Name, swapValue);
            }
            else
            {
                sb.AppendFormat("Footage\", comp: \"{0}\", file: \"{1}\" }}", Name, swapValue.ToAfterEffectsPath());
            }
            return sb.ToString();
        }

        public dynamic ToPoco()
        {
            return new
            {
                pk_TemplateScriptItemId = pk_TemplateScriptItemId,
                fk_TemplateScriptId = fk_TemplateScriptId,
                fk_TemplateScriptItemTypeId = fk_TemplateScriptItemTypeId,
                fk_TemplateScriptItemCategoryId = fk_TemplateScriptItemCategoryId,
                Name = Name,
                Description = Description,
                ItemWidth = ItemWidth,
                ItemHeight = ItemHeight
            };
        }
    }
}
