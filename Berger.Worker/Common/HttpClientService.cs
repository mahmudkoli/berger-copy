using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Berger.Worker.Common
{
    public class HttpClientService:IHttpClientService
    {
        private readonly ILogger<HttpClientService> _logger;

        public HttpClientService(ILogger<HttpClientService> logger)
        {
            _logger = logger;
        }

        public string GetHttpResponse(string url)
        {
            try
            {
                HttpClient client = new HttpClient();
                var RequestMessage = HttpClientAuthentication.Authenticate(url);
                //make the request
                var task = client.SendAsync(RequestMessage);
                
                var response = task.Result;
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    return responseBody;
                }
                else
                {
                    throw new ArgumentException("Something Wrong with host");
                }
                
            }
            catch (HttpRequestException httpEx)
            {
                //_logger.LogCritical(httpEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.InnerException.Message);
                throw;
            }
        }

    }
}
