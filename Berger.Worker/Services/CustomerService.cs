using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Berger.Data.MsfaEntity;
using Berger.Data.MsfaEntity.SAPTables;
using Berger.Worker.Common;
using Berger.Worker.JSONParser;
using Berger.Worker.Query;
using Berger.Worker.ViewModel;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Berger.Worker.Services
{
    public class CustomerService:ICustomerService
    {
        private HttpClient _client;
        private CustomerQuery _query;
        private readonly IRepository<DealerInfo> _repo;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientService _httpService;



        public CustomerService(IRepository<DealerInfo> repo, 
            IServiceScopeFactory serviceScopeFactory, ApplicationDbContext context, IHttpClientService httpClientService)
        {
            _client = new HttpClient();
            _query = new CustomerQuery();
            _repo = repo;
            _serviceScopeFactory = serviceScopeFactory;
            _context = context;
            _httpService = httpClientService;

        }

        public async Task<int> getData()
        {
            try
            {
                string url = _query.GetAllCustomer();
                
                var responseBody = _httpService.GetHttpResponse(_client, url);

                var mappedData  = Parser<CustomerModel>.ParseJson(responseBody);

                 var listo =  mappedData.results.ToMap<CustomerModel, DealerInfo>();

                 var dbData = await _repo.GetAllAsync();

                 var listoInDbData = listo.Except(dbData).Any();
                 var listoNotInDbData = dbData.Except(listo).ToList();

                //_context.DealerInfos.AddRange(listo);
                await _repo.CreateListAsync(listo);

            
            }
            catch (Exception ex)
            {

                throw;
            }

           return 1;
        }
    }
}
