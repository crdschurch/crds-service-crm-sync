using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Models.Models;
using Newtonsoft.Json.Linq;

namespace ExternalSync.Hubspot
{
    public class HubspotClient : IHubspotClient
    {
        public async Task<bool> SyncGroupParticipationData(List<MpGroupMembership> mpGroupParticipations)
        {
            try
            {
                //public PushpayClient(IPushpayTokenService pushpayTokenService, IRestClient restClient = null)
                //{
                //    _pushpayTokenService = pushpayTokenService;
                //    _restClient = restClient ?? new RestClient();
                //    _restClient.BaseUrl = apiUri;
                //}

                //public async Task<List<PushpayPaymentDto>> GetDonations(string settlementKey)
                //{
                //    var resource = $"settlement/{settlementKey}/payments";
                //    var data = await CreateAndExecuteRequest(resource, Method.GET, donationsScope, null, true);
                //    return JsonConvert.DeserializeObject<List<PushpayPaymentDto>>(data);
                //}

                var testParticipant = mpGroupParticipations.First();

                //start with adding one contact for now
                var restClient = new RestClient();
                restClient.BaseUrl = new Uri("https://api.hubapi.com");
                var resource =
                    $"/contacts/v1/contact/batch/<key goes here>";


                var request = new RestRequest(Method.POST)
                {
                    //Resource = uriOrResource.StartsWith(apiUri.AbsoluteUri, StringComparison.Ordinal) ? uriOrResource.Replace(apiUri.AbsoluteUri, "") : uriOrResource
                    Resource = resource
                };

                request.AddHeader("Accept", "application/json");
                //var x = JsonConvert.SerializeObject(testParticipant.GroupMembership);

                var body = SerializeContactsArray(mpGroupParticipations);


                request.AddParameter("application/json", body, ParameterType.RequestBody);

                //if (queryParams != null)
                //{
                //    foreach (QueryParameter entry in queryParams)
                //    {
                //        // do something with entry.Value or entry.Key
                //        request.AddQueryParameter(entry.Key, entry.Value);
                //    }
                //}

                var response = restClient.Execute(request);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return true;
        }

        public string SerializeContactsArray(List<MpGroupMembership> mpGroupMemberships)
        {
            //// create list of xml nodes
            //var nodes = new List<XElement>();

            //foreach (var mpGroupMembership in mpGroupMemberships)
            //{
            //    var membershipElement = new XElement();
            //}


            //var membershipsArray = new XElement(
            //    new XElement("test"));

            var membership = new JArray();

            //foreach (var mpGroupMembership in mpGroupMemberships)
            //{
            //    var membershipObject = new JObject(
            //        new JProperty("email", mpGroupMembership.ContactEmail,
            //        new JObject(
            //            new JProperty("properties",
            //                new JArray { new JObject
            //                            (new JProperty("GroupMembership", mpGroupMembership.GroupMembership)) }
            //                ))));

            //    membership.Add(membershipObject);
            //}

            //foreach (var mpGroupMembership in mpGroupMemberships)
            //{
            //    var membershipObject = new JObject(
            //        new JProperty("email", mpGroupMembership.ContactEmail),
            //        new JProperty("properties",
            //            new JObject(
            //                new JProperty("properties",
            //                    new JArray { new JObject
            //                        (new JProperty("GroupMembership", mpGroupMembership.GroupMembership)) }
            //                ))));

            //    membership.Add(membershipObject);
            //}

            foreach (var mpGroupMembership in mpGroupMemberships)
            {
                var membershipObject = new JObject(
                    new JProperty("email", mpGroupMembership.ContactEmail),
                    new JProperty("properties",
                        new JArray { new JObject( new JProperty("property", "GroupMembership"), //mpGroupMembership.GroupMembership) }
                                     new JProperty("value", mpGroupMembership.GroupMembership)) }
                            )
                    );

                membership.Add(membershipObject);
            }

            return membership.ToString();
        }
    }
}
