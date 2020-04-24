using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models.Models;

namespace ExternalSync.Hubspot
{
    public interface IHubspotClient
    {
        Task<bool> SyncGroupParticipationData(List<MpGroupMembership> mpGroupParticipations);
    }
}
