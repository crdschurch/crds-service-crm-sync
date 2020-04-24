using Newtonsoft.Json;

namespace Models.Models
{
    public class MpGroupMembership
    {
        [JsonProperty(PropertyName = "ContactEmail")]
        public string ContactEmail { get; set; }

        [JsonProperty(PropertyName = "GroupMembership")]
        public string GroupMembership { get; set; }
    }
}
