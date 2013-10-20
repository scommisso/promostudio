using PromoStudio.Common.Enumerations;
using System;

namespace PromoStudio.Common.Models
{
    public class VoiceActor
    {
        public int pk_VoiceActorId { get; set; }
        public sbyte fk_VoiceActorStatusId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Biography { get; set; }
        public string SampleFilePath { get; set; }
        public string PhotoFilePath { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        public VoiceActorStatus Status
        {
            get { return (VoiceActorStatus)fk_VoiceActorStatusId; }
            set { fk_VoiceActorStatusId = (sbyte)value; }
        }

        public dynamic ToPoco()
        {
            return new
            {
                pk_VoiceActorId = pk_VoiceActorId,
                fk_VoiceActorStatusId = fk_VoiceActorStatusId,
                UserName = UserName,
                FullName = FullName,
                Biography = Biography,
                SampleFilePath = SampleFilePath,
                PhotoFilePath = PhotoFilePath,
                DateCreated = DateCreated,
                DateUpdated = DateUpdated
            };
        }
    }
}
