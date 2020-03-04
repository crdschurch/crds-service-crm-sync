﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ExternalSync.Models;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace ExternalSync.TokenService
{
    public class TokenService : ITokenService
    {
        private readonly string clientId = Environment.GetEnvironmentVariable("PUSHPAY_CLIENT_ID");
        private readonly string clientSecret = Environment.GetEnvironmentVariable("PUSHPAY_CLIENT_SECRET");
        private readonly Uri authUri = new Uri(Environment.GetEnvironmentVariable("PUSHPAY_AUTH_ENDPOINT") ?? "https://auth.pushpay.com/pushpay-sandbox/oauth");

        private readonly IRestClient _restClient;

        public TokenService(IRestClient restClient = null)
        {
            _restClient = restClient ?? new RestClient();
            _restClient.BaseUrl = authUri;
        }

        public async Task<OAuth2TokenResponse> GetOAuthToken(string scope = "read")
        {
            _restClient.Authenticator = new HttpBasicAuthenticator(clientId, clientSecret);
            var request = new RestRequest(Method.POST)
            {
                Resource = "token"
            };
            request.AddParameter("grant_type", "client_credentials");
            request.AddParameter("scope", scope);

            var response = await _restClient.ExecuteTaskAsync<OAuth2TokenResponse>(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var tokenJson = response.Content;
                var tokens = JsonConvert.DeserializeObject<OAuth2TokenResponse>(tokenJson);
                return tokens;
            }
            else
            {
                throw new Exception($"Authentication was not successful {response.StatusCode}");
            }
        }
    }
}
