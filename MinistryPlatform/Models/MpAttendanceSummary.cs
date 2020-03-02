using Crossroads.Web.Common.MinistryPlatform;
using Newtonsoft.Json;
using System;

namespace MinistryPlatform.Models
{
    //[MpRestApiTable(Name = "Group_Participants")]
    public class MpAttendanceSummary
    {
        [JsonProperty(PropertyName = "ParticipantId")]
        public int ParticipantId { get; set; }

        [JsonProperty(PropertyName = "Nickname")]
        public string Nickname { get; set; }

        [JsonProperty(PropertyName = "LastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "LastAttended")]
        public DateTime? LastAttended { get; set; }

        [JsonProperty(PropertyName = "MissedTotal")]
        public int? MissedTotal { get; set; }

        [JsonProperty(PropertyName = "MissedConsecutive")]
        public int? MissedConsecutive { get; set; }
    }
}
