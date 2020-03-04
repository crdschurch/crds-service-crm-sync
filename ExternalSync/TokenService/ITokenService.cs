using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ExternalSync.Models;

namespace ExternalSync.TokenService
{
    public interface ITokenService
    {
        Task<OAuth2TokenResponse> GetOAuthToken(string scope = "read");
    }
}
