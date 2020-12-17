namespace Berger.Common.HttpClient
{
    public interface IHttpClientService
    {
        public string GetHttpResponse(string url,string username, string password);

    }
}
