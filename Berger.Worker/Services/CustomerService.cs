using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Berger.Data.MsfaEntity.SAPTables;
using Berger.Worker.Common;
using Berger.Worker.JSONParser;
using Berger.Worker.Query;
using Berger.Worker.ViewModel;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Repositories;

namespace Berger.Worker.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly HttpClient _client;
        private readonly CustomerQuery _query;
        private readonly IRepository<DealerInfo> _repo;
        private readonly IHttpClientService _httpService;
        private readonly IDataEqualityComparer<DealerInfo> _dataCoparer;



        public CustomerService(IRepository<DealerInfo> repo, IHttpClientService httpClientService, IDataEqualityComparer<DealerInfo> comparer)
        {
            _client = new HttpClient();
            _query = new CustomerQuery();
            _repo = repo;
            _httpService = httpClientService;
            _dataCoparer = comparer;

        }

        public async Task<int> getData()
        {
            string url = _query.GetAllCustomer();

            var responseBody = _httpService.GetHttpResponse(_client, url);
            var parsedDataFromApi = Parser<CustomerModel>.ParseJson(responseBody);

            var mappedDataFromApi = parsedDataFromApi.results.ToMap<CustomerModel, DealerInfo>();
            var dataFromDatabase = await _repo.GetAllAsync();
            var fromDatabase = dataFromDatabase as DealerInfo[] ?? dataFromDatabase.ToArray();

            List<string> insertDeleteKeys = new List<string>();
            if (fromDatabase.Length != mappedDataFromApi.Count)
            {
                
                var data = await _dataCoparer.GetNewDatasetOfApi(x => x.CompositeKey, mappedDataFromApi, fromDatabase);
                await _repo.CreateListAsync(data.Item2);
                insertDeleteKeys.AddRange(data.Item1);

                //Filter records needs to be inserted from api list

                //var newKeyInApiList = await InsertNewDataList(mappedDataFromApi, fromDatabase, insertDeleteKeys);

                //Filter Record for delete, database e ache but api list e nai
                var deletedList = await _dataCoparer.GetDeletedDataOfApi(x => x.CompositeKey, mappedDataFromApi, fromDatabase);
                await _repo.UpdateListAsync(deletedList.Item2);
                //insertDeleteKeys.AddRange(deletedList.Item1);


                // Data Update Check
                if (insertDeleteKeys.Count > 0)
                {
                    var updatedData = mappedDataFromApi.Where(a => !insertDeleteKeys.Contains(a.CompositeKey))
                        .ToList();
                    //updatedData.GroupBy(x=>x.CustomerNo).Select(y => y.FirstOrDefault()).ToList();
                    await _repo.UpdateListAsync(updatedData);
                }

            }
            else
            {
                await _repo.UpdateListAsync(mappedDataFromApi);
            }

            //await _repo.CreateListAsync(mappedDataFromApi);

            return 1;
        }

        //private async Task<List<int>> InsertNewDataList(List<DealerInfo> mappedDataFromApi, DealerInfo[] fromDatabase, List<int> insertDeleteKeys)
        //{
        //    var newKeyInApiList = mappedDataFromApi.Select(s => s.CustomerNo)
        //        .Except(fromDatabase.Select(s => s.CustomerNo)).ToList();
        //    insertDeleteKeys.AddRange(newKeyInApiList);

        //    var newDataFromApi = mappedDataFromApi.Where(a => newKeyInApiList.Contains(a.CustomerNo)).Distinct().ToList();
        //    newDataFromApi = newDataFromApi.GroupBy(x => x.CustomerNo).Select(y => y.FirstOrDefault()).ToList();
        //    await _repo.CreateListAsync(newDataFromApi);
        //    return newKeyInApiList;
        //}
    }
}
