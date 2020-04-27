using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Models.Models;
using Newtonsoft.Json.Linq;
using System.Net;

namespace ExternalSync.Hubspot
{
    public class HubspotClient : IHubspotClient
    {
        public async Task<bool> SyncGroupParticipationData(List<MpGroupMembership> mpGroupParticipations)
        {
            bool isSuccessful = true;
            try
            {
                // Create rest client
                var restClient = new RestClient();
                restClient.BaseUrl = new Uri("https://api.hubapi.com");

                // Setup appropriate HubSpot URL with api key
                var request = new RestRequest(Method.POST);
                var hubSpotApiKey = Environment.GetEnvironmentVariable("HUBSPOT_API_KEY");
                request.Resource = $"/contacts/v1/contact/batch/?hapikey={hubSpotApiKey}";
                request.AddHeader("Accept", "application/json");

                // Send JSON objects to HubSpot in batches of 100 or less
                int recordCount = 0;
                do
                {
                    var recordsToProcess = mpGroupParticipations.Skip(recordCount).Take(1);
                    recordCount += recordsToProcess.Count();
                    var body = SerializeContactsArray(recordsToProcess.ToList<MpGroupMembership>());
                    request.AddOrUpdateParameter("application/json", body, ParameterType.RequestBody);

                    var response = restClient.Execute(request);
                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        isSuccessful = false;
                        Console.WriteLine(body);
                        //TODO: Log a message and the data
                    }
                } while (recordCount < mpGroupParticipations.Count);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            // Return true if no 400 errors were encountered, else return false
            return isSuccessful;
        }

        public string SerializeContactsArray(List<MpGroupMembership> mpGroupMemberships)
        {
            var membership = new JArray();

            foreach (var mpGroupMembership in mpGroupMemberships)
            {
                var membershipObject = new JObject(
                    new JProperty("email", mpGroupMembership.ContactEmail.Trim()),
                    new JProperty("properties",
                        new JArray { 
                            new JObject( 
                                new JProperty("property", "groupMembership"),
                                new JProperty("value", mpGroupMembership.GroupMembership)
                            )
                        }
                    )
                );

                membership.Add(membershipObject);
            }

            return membership.ToString();
        }
    }
}
