using Newtonsoft.Json;

namespace Crossroads.Service.Contact.Models
{
    public class GroupParticipantDto
    {
        [JsonProperty(PropertyName = "groupParticipantId")]
        public int GroupParticipantId { get; set; }

        [JsonProperty(PropertyName = "groupId")]
        public int GroupId { get; set; }

        [JsonProperty(PropertyName = "participantId")]
        public int ParticipantId { get; set; }

        [JsonProperty(PropertyName = "contactId")]
        public int ContactId { get; set; }

        [JsonProperty(PropertyName = "nickname")]
        public string Nickname { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "isLeader")]
        public bool IsLeader { get; set; }

        [JsonProperty(PropertyName = "isMember")]
        public bool IsMember { get; set; }
    }
}
