using Newtonsoft.Json;

namespace Crossroads.Service.Contact.Models
{
    public class EventParticipantDto
    {
        [JsonProperty(PropertyName = "participantId")]
        public int ParticipantId { get; set; }

        [JsonProperty(PropertyName = "eventParticipantId")]
        public int EventParticipantId { get; set; }

        [JsonProperty(PropertyName = "participantStatusId")]
        public int ParticipantStatusId { get; set; }

        [JsonProperty(PropertyName = "nickname")]
        public string Nickname { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "attending")]
        public bool Attending { get; set; }
    }
}
