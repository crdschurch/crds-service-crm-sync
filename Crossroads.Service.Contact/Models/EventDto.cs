using Newtonsoft.Json;
using System;

namespace Crossroads.Service.Contact.Models
{
    public class EventDto
    {
        [JsonProperty(PropertyName = "eventId")]
        public int EventId { get; set; }

        [JsonProperty(PropertyName = "eventTitle")]
        public string EventTitle { get; set; }

        [JsonProperty(PropertyName = "congregationName")]
        public string CongregationName { get; set; }

        [JsonProperty(PropertyName = "eventStartDate")]
        public DateTime EventStartDate { get; set; }

        [JsonProperty(PropertyName = "eventEndDate")]
        public DateTime EventEndDate { get; set; }

        [JsonProperty(PropertyName = "groupId")]
        public int? GroupId { get; set; }

        [JsonProperty(PropertyName = "groupName")]
        public string GroupName { get; set; }
    }
}
