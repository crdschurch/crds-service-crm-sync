using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;

namespace Crossroads.Crm.AzureFunctions
{
    public static class RunEndpointUrl
    {
        [FunctionName("RunEndpointUrl")]
        public static async void RunAsync(
            [TimerTrigger("0 0 0,12 * * *")]TimerInfo myTimer,
            ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            string endpointUrl = Environment.GetEnvironmentVariable("ENDPOINT_URL");
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, endpointUrl);
            HttpResponseMessage response = await httpClient.SendAsync(request);
            HttpStatusCode statusCode = response.StatusCode;

            log.LogInformation($"{endpointUrl} returned a {statusCode} response");
        }
    }
}
