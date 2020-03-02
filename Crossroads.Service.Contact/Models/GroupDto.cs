using Newtonsoft.Json;
using System;

namespace Crossroads.Service.Contact.Models
{
    public class GroupDto
    {
        [JsonProperty(PropertyName = "groupId")]
        public int GroupId { get; set; }

        [JsonProperty(PropertyName = "groupName")]
        public string GroupName { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string GroupDescription { get; set; }

        [JsonProperty(PropertyName = "eventId")]
        public int EventId { get; set; }

        [JsonProperty(PropertyName = "groupParticipants")]
        public int GroupParticipants { get; set; }

        [JsonProperty(PropertyName = "checkedIn")]
        public int CheckedIn { get; set; }
    }
}
