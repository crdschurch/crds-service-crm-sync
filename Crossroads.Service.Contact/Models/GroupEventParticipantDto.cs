using Newtonsoft.Json;

namespace Crossroads.Service.Contact.Models
{
    public class GroupEventParticipantDto
    {
        [JsonProperty(PropertyName = "participantId")]
        public int ParticipantId { get; set; }

        [JsonProperty(PropertyName = "contactId")]
        public int ContactId { get; set; }

        [JsonProperty(PropertyName = "groupId")]
        public int GroupId { get; set; }

        [JsonProperty(PropertyName = "eventId")]
        public int EventId { get; set; }

        [JsonProperty(PropertyName = "groupParticipantId")]
        public int GroupParticipantId { get; set; }
                
        [JsonProperty(PropertyName = "eventParticipantId")]
        public int? EventParticipantId { get; set; }

        [JsonProperty(PropertyName = "nickname")]
        public string Nickname { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "isLeader")]
        public bool IsLeader { get; set; }

        [JsonProperty(PropertyName = "isMember")]
        public bool IsMember { get; set; }
        
        [JsonProperty(PropertyName = "attending")]
        public bool Attending { get; set; }

        [JsonProperty(PropertyName = "groupDescription")]
        public string GroupDescription { get; set; }
    }
}
