using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Berger.Common.HttpClient
{
    public static class HttpClientAuthentication
    {
        public static HttpRequestMessage Authenticate(string url, string username, string password)
        {
            
            var authenticationString = $"{username}:{password}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);

            return requestMessage;

        }

    }
}
