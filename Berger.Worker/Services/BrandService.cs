using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Berger.Common.HttpClient;
using Berger.Common.JSONParser;
using Berger.Data.MsfaEntity.SAPTables;
using Berger.Worker.Common;
using Berger.Worker.Model;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Berger.Worker.Services
{
    public class BrandService : IBrandService
    {
        
        private readonly WorkerSettingsModel _appSettings;
        private readonly IRepository<BrandInfo> _repo;
        private readonly IHttpClientService _httpService;
        private readonly IDataEqualityComparer<BrandInfo> _dataComparer;
        private readonly ILogger<BrandInfo> _logger;



        public BrandService(IRepository<BrandInfo> repo, IHttpClientService httpClientService, IDataEqualityComparer<BrandInfo> comparer,ILogger<BrandInfo> logger, IOptions<WorkerSettingsModel> appSettings)
        {
            _appSettings = appSettings.Value;
            _repo = repo;
            _httpService = httpClientService;
            _dataComparer = comparer;
            _logger = logger;

        }

        public async Task<int> GetBrandData()
        {
            try
            {
                
                string url = $"{_appSettings.BaseAddress}{_appSettings.BrandUrl}";
                string username = _appSettings.UserName;
                string password = _appSettings.Password;
                _logger.LogInformation($"{DateTime.Now} URL Building Successful... {url}");
                var responseBody = _httpService.GetHttpResponse(url, username, password);

                _logger.LogInformation($"Got http response.. Parsing Started");
                var parsedDataFromApi = Parser<BrandRootModel>.ParseJson(responseBody);

                _logger.LogInformation($"Parsing completed successfully. Total {parsedDataFromApi.Results.Count} record parsed!!");
                var modelData = parsedDataFromApi.Results.Select(x => x.ToModel()).ToList();
                var mappedDataFromApi = modelData.ToMap<BrandModel, BrandInfo>();

                _logger.LogInformation($"Fetching existing data from database....");
                var dataFromDatabase = await _repo.FindAllAsync(x=>x.IsDeleted ==false);
                if (dataFromDatabase != null)
                {
                    IEnumerable<BrandInfo> dealerInfos = dataFromDatabase.ToList();
                    _logger.LogInformation($"Total data fetched from database is {dealerInfos.Count()}");

                    var fromDatabase = dealerInfos.ToList();
                    mappedDataFromApi = mappedDataFromApi.GroupBy(x=>x.CompositeKey).Select(y => y.FirstOrDefault()).ToList();
                    List<string> insertDeleteKeys = new List<string>();
                    if (fromDatabase.Count != mappedDataFromApi.Count)
                    {
                        //Insert
                        //Get records from api which is not present in current database
                        var data = await _dataComparer.GetNewDatasetOfApi(x => x.CompositeKey, mappedDataFromApi,
                            fromDatabase);
                        _logger.LogInformation($"Total new record found in API : {data.Item2.Count}");
                        var res =await _repo.CreateListAsync(data.Item2);
                        _logger.LogInformation($"Total Data inserted: {res.Count}");
                        insertDeleteKeys.AddRange(data.Item1);


                        //Delete
                        //Get the Records which are present in Database but not present in API. Means data deleted in API end. 
                        var deletedList =
                            await _dataComparer.GetDeletedDataOfApi(x => x.CompositeKey, mappedDataFromApi,
                                fromDatabase);
                        foreach (var dealerInfo in deletedList.Item2)
                        {
                            dealerInfo.IsDeleted = true;
                            dealerInfo.IsActive = false;
                        }
                        _logger.LogInformation($"Total deletion record found: {deletedList.Item2.Count}");
                        var delres = await _repo.UpdateListAsync(deletedList.Item2);
                        if(delres != null)
                         _logger.LogInformation($"Total delete record updated: {delres.Count}");
                        insertDeleteKeys.AddRange(deletedList.Item1);


                        // Update
                        if (insertDeleteKeys.Count > 0)
                        {
                            var updatedData = mappedDataFromApi.Where(a => !insertDeleteKeys.Contains(a.CompositeKey))
                                .ToList();
                            if(updatedData.Any())
                            {
                                var updateres = await _repo.UpdateListAsync(updatedData);
                                _logger.LogInformation($"Total record updated form api: {updateres.Count}");
                            }
                        }
                    }
                    else
                    {
                        _logger.LogInformation("No new or Delete data found!!!Updating Data....Wait");
                       dataFromDatabase = dataFromDatabase
                            .Where(a => mappedDataFromApi.Select(b => b.CompositeKey).Contains(a.CompositeKey))
                            .ToList();
                        foreach (var dealerInfo in mappedDataFromApi)
                        {
                            var IsMatch = dataFromDatabase.FirstOrDefault(a => a.CompositeKey == dealerInfo.CompositeKey);
                            if (IsMatch != null)
                            {
                                dealerInfo.Id = IsMatch.Id;
                            }
                        }
                        var upres = await _repo.UpdateListiAsync(mappedDataFromApi);
                        _logger.LogInformation($"Total record updated: {upres}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
          

            return 1;
        }

        //private async Task<List<int>> InsertNewDataList(List<Brand> mappedDataFromApi, Brand[] fromDatabase, List<int> insertDeleteKeys)
        //{
        //    var newKeyInApiList = mappedDataFromApi.Select(s => s.BrandNo)
        //        .Except(fromDatabase.Select(s => s.BrandNo)).ToList();
        //    insertDeleteKeys.AddRange(newKeyInApiList);

        //    var newDataFromApi = mappedDataFromApi.Where(a => newKeyInApiList.Contains(a.BrandNo)).Distinct().ToList();
        //    newDataFromApi = newDataFromApi.GroupBy(x => x.BrandNo).Select(y => y.FirstOrDefault()).ToList();
        //    await _repo.CreateListAsync(newDataFromApi);
        //    return newKeyInApiList;
        //}
    }
}
