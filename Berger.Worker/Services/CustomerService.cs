using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Berger.Worker.Common;
using Berger.Worker.JSONParser;
using Berger.Worker.Query;
using Berger.Worker.ViewModel;

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

        public async Task<int> getData()
        {
            try
            {
                string url = _query.GetAllCustomer();
                var requestMessage = HttpClientAuthentication.Authenticate(url);
                //make the request
                var task = _client.SendAsync(requestMessage);
                var response = task.Result;
                response.EnsureSuccessStatusCode();
                string responseBody = response.Content.ReadAsStringAsync().Result;
                Parser<CustomerModel> objParser = new Parser<CustomerModel>();
                objParser.ParseJson(responseBody);
            }
            catch (Exception ex)
            {

                throw;
            }

           return 1;
        }
    }
}
