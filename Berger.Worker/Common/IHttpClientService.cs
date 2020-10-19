using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Berger.Worker.Common
{
    public interface IHttpClientService
    {
        public string GetHttpResponse(HttpClient client, string url);

    }
}
