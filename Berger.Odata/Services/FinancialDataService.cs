using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Berger.Common.HttpClient;
using Berger.Common.JSONParser;
using Berger.Odata.Model;
using Microsoft.Extensions.Options;

namespace Berger.Odata.Services
{
    public class FinancialDataService: IFinancialDataService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly ODataSettingsModel _appSettings;

        public FinancialDataService(IHttpClientService httpClientService, IOptions<ODataSettingsModel> appSettings)
        {
            _httpClientService = httpClientService;
            _appSettings = appSettings.Value;
        }

        private async Task<IList<FinanceDataModel>> GetFinanceData(string query)
        {
            string fullUrl = $"{_appSettings.BaseAddress}{_appSettings.FinancialUrl}{query}";

            var responseBody = _httpClientService.GetHttpResponse(fullUrl, _appSettings.UserName, _appSettings.Password);
            var parsedData = Parser<FinanceDateRootModel>.ParseJson(responseBody);
            var data = parsedData.Results.Select(x => x.ToModel()).ToList();

            return data;
        }


        public async Task GetAllFinanceData()
        {
            //$"filter{}"
        }

    }
}
