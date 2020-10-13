using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Berger.Worker.Query;

namespace Berger.Worker.Services
{
    public class CustomerService:ICustomerService
    {
        private HttpClient _client;
        private CustomerQuery _query;


        public CustomerService()
        {
            _client = new HttpClient();
            _query = new CustomerQuery();
        }

        public void getData()
        {
           string url= _query.GetAllCustomer();
           string userName = "Bpbldev1";
           string pass = "Bpbldev2017";
           var authenticationString = $"{userName}:{pass}";
           var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));

           var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
           requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
          

           //make the request
           var task =  _client.SendAsync(requestMessage);
           var response = task.Result;
           response.EnsureSuccessStatusCode();
           string responseBody = response.Content.ReadAsStringAsync().Result;
           Console.WriteLine(responseBody);
        }
    }
}
