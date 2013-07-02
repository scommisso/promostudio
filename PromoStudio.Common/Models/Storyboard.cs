using PromoStudio.Common.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoStudio.Common.Models
{
    public class Storyboard
    {
        private List<StoryboardItem> _storyboardItems = new List<StoryboardItem>();

        public long pk_StoryboardId { get; set; }
        public sbyte fk_StoryboardStatusId { get; set; }
        public int? fk_OrganizationId { get; set; }
        public int? fk_VerticalId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<StoryboardItem> Items {
            get { return _storyboardItems; }
            set { _storyboardItems = value; }
        }

        public StoryboardStatus Status
        {
            get { return (StoryboardStatus)fk_StoryboardStatusId; }
            set { fk_StoryboardStatusId = (sbyte)value; }
        }

        public dynamic ToPoco()
        {
            return new
            {
                pk_StoryboardId = pk_StoryboardId,
                fk_StoryboardStatusId = fk_StoryboardStatusId,
                fk_OrganizationId = fk_OrganizationId,
                fk_VerticalId = fk_VerticalId,
                Name = Name,
                Description = Description
            };
        }
    }
}
