using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        
        private readonly CustomerQuery _query;
        private readonly IRepository<DealerInfo> _repo;
        private readonly IHttpClientService _httpService;
        private readonly IDataEqualityComparer<DealerInfo> _dataComparer;



        public CustomerService(IRepository<DealerInfo> repo, IHttpClientService httpClientService, IDataEqualityComparer<DealerInfo> comparer)
        {
            _query = new CustomerQuery();
            _repo = repo;
            _httpService = httpClientService;
            _dataComparer = comparer;

        }

        public async Task<int> getData()
        {
            try
            {
                string url = _query.GetAllCustomer();
                var responseBody = _httpService.GetHttpResponse(url);
                var parsedDataFromApi = Parser<CustomerModel>.ParseJson(responseBody);

                var mappedDataFromApi = parsedDataFromApi.results.ToMap<CustomerModel, DealerInfo>();
                var dataFromDatabase = await _repo.GetAllAsync();
                var fromDatabase = dataFromDatabase as DealerInfo[] ?? dataFromDatabase.ToArray();

                List<string> insertDeleteKeys = new List<string>();
                if (fromDatabase.Length != mappedDataFromApi.Count)
                {
                    //Insert
                    //Get records from api which is not present in current database
                    var data = await _dataComparer.GetNewDatasetOfApi(x => x.CompositeKey, mappedDataFromApi, fromDatabase);
                    await _repo.CreateListAsync(data.Item2);
                    insertDeleteKeys.AddRange(data.Item1);


                    //Delete
                    //Get the Records which are present in Database but not present in API. Means data deleted in API end. 
                    var deletedList = await _dataComparer.GetDeletedDataOfApi(x => x.CompositeKey, mappedDataFromApi, fromDatabase);
                    foreach (var dealerInfo in deletedList.Item2)
                    {
                        dealerInfo.IsDeleted = true;
                        dealerInfo.IsActive = false;
                    }
                    await _repo.UpdateListAsync(deletedList.Item2);
                    insertDeleteKeys.AddRange(deletedList.Item1);


                    // Update
                    if (insertDeleteKeys.Count > 0)
                    {
                        var updatedData = mappedDataFromApi.Where(a => !insertDeleteKeys.Contains(a.CompositeKey))
                            .ToList();
                        await _repo.UpdateListAsync(updatedData);
                    }

                }
                else
                {
                    await _repo.UpdateListAsync(mappedDataFromApi);
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
          

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
