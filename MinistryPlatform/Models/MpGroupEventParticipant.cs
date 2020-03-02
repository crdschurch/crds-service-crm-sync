using Newtonsoft.Json;

namespace MinistryPlatform.Models
{
    public class MpGroupEventParticipant
    {
        [JsonProperty(PropertyName = "Participant_ID")]
        public int ParticipantId { get; set; }

        [JsonProperty(PropertyName = "Contact_ID")]
        public int ContactId { get; set; }

        [JsonProperty(PropertyName = "Group_ID")]
        public int GroupId { get; set; }

        [JsonProperty(PropertyName = "Event_ID")]
        public int EventId { get; set; }

        [JsonProperty(PropertyName = "Group_Participant_ID")]
        public int GroupParticipantId { get; set; }

        [JsonProperty(PropertyName = "Event_Participant_ID")]
        public int? EventParticipantId { get; set; }

        [JsonProperty(PropertyName = "Nickname")]
        public string Nickname { get; set; }

        [JsonProperty(PropertyName = "Last_Name")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "Is_Leader")]
        public bool IsLeader { get; set; }

        [JsonProperty(PropertyName = "Is_Member")]
        public bool IsMember { get; set; }
        
        [JsonProperty(PropertyName = "Participation_Status_ID")]
        public int? ParticipantStatusId { get; set; }

        [JsonProperty(PropertyName = "Group_Description")]
        public string GroupDescription { get; set; }

        [JsonProperty(PropertyName = "Domain_ID")]
        public int DomainId { get; set; }

        [JsonProperty(PropertyName = "Group_Role_ID")]
        public int GroupRoleId { get; set; }

    }
}
