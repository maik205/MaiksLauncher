using MojangSharpCore;
using MojangSharpCore.Api;
using MojangSharpCore.Endpoints;
using MojangSharpCore.Responses;
using System;
using System.IO;
using static MojangSharpCore.Endpoints.Statistics;
using static MojangSharpCore.Responses.ChallengesResponse;
using static MojangSharpCore.Responses.NameHistoryResponse;

namespace Authenticator
{
    public class MojangAuthenticate
    {
        public string MojangAccessToken(string email, string password)
        {
            AuthenticateResponse auth = new Authenticate(new Credentials() { Username = email, Password = password }).PerformRequestAsync().Result;
            return auth.AccessToken;
        }

    }
}
