using Newtonsoft.Json;
using System;

namespace Crossroads.Service.Contact.Models
{
    public class AttendanceSummaryDto
    {
        [JsonProperty(PropertyName = "participantId")]
        public int ParticipantId { get; set; }

        [JsonProperty(PropertyName = "nickname")]
        public string Nickname { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "lastAttended")]
        public DateTime? LastAttended { get; set; }

        [JsonProperty(PropertyName = "missedTotal")]
        public int? MissedTotal { get; set; }

        [JsonProperty(PropertyName = "missedConsecutive")]
        public int? MissedConsecutive { get; set; }
    }
}
