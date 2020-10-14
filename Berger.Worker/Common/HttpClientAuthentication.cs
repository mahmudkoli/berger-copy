using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Berger.Worker.Common
{
    public static class HttpClientAuthentication
    {
        public static HttpRequestMessage Authenticate(string url)
        {
            
            string userName = "Bpbldev1";
            string pass = "Bpbldev2017";
            var authenticationString = $"{userName}:{pass}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);

            return requestMessage;

        }

    }
}
