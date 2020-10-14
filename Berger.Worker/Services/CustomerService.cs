using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Berger.Data.MsfaEntity;
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
        private readonly ApplicationDbContext _db;


        public CustomerService(ApplicationDbContext db)
        {
            _client = new HttpClient();
            _query = new CustomerQuery();
            _db = db;
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
                var mappedData = objParser.ParseJson(responseBody);

                //foreach (var item in mappedData.results)
                //{
                //    _db.CMUsers.Add()
                //}

            }
            catch (Exception ex)
            {

                throw;
            }

           return 1;
        }
    }
}
