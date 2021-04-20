﻿using Microsoft.Extensions.Logging;
using System;
using HTTP = System.Net.Http;

namespace Berger.Common.HttpClient
{
    public class HttpClientService : IHttpClientService
    {
        private readonly ILogger<HttpClientService> _logger;

        public HttpClientService(ILogger<HttpClientService> logger)
        {
            _logger = logger;
        }

        public string GetHttpResponse(string url, string username, string password)
        {

            using (HTTP.HttpClientHandler clientHandler = new HTTP.HttpClientHandler())
            {

                try
                {
                    clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                    HTTP.HttpClient client = new HTTP.HttpClient(clientHandler);
                    var RequestMessage = HttpClientAuthentication.Authenticate(url, username, password);

                    _logger.LogInformation($"Http request started with authentication");
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
                        _logger.LogInformation($"Something Wrong with host STATUS CODE: {response.StatusCode}");
                        //return "";
                        throw new Exception("Failed to fetched data from SAP.");
                    }

                }
                catch (HTTP.HttpRequestException httpEx)
                {
                    _logger.LogCritical(httpEx.Message);
                    //throw;
                    throw new Exception("Failed to fetched data from SAP.", httpEx);
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex.InnerException.Message);
                    //throw;
                    throw new Exception("Failed to fetched data from SAP.", ex);
                }

            }
        }
    }
}
