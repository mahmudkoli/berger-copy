using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Berger.Common.HttpClient;
using Berger.Common.JSONParser;
using Berger.Odata.Common;
using Berger.Odata.Model;
using ODataHttpClient;
using ODataHttpClient.Models;

namespace Berger.Odata.Services
{
    public class SalesDataService : ISalesData
    {
        private readonly IHttpClientService _httpClient;

        public SalesDataService(IHttpClientService httpClient)
        {
            _httpClient = httpClient;
        }

        public Dictionary<string, string> SalesDataAlias = new Dictionary<string, string>
        {
            {"bukrs", "CompanyCode"},
            {"spart", "Division"}
        };

        public async Task<IEnumerable<SalesDataModel>> GetSalesData()
        {
            try
            {
                string fullUrl =
                    $"{OdataUrlBuilder.SalesUrl}&$filter=kunrg eq '24' and fkdat gt datetime%272011-09-01T00:00:00%27 and fkdat lt datetime%272011-09-08T00:00:00%27";

                var responseBody = _httpClient.GetHttpResponse(fullUrl, OdataUrlBuilder.userName,
                       OdataUrlBuilder.password);
                var parsedData = Parser<SalesDataRootModel>.ParseJson(responseBody);
                var data = parsedData.results.Select(a => new SalesDataModel()
                {
                    CompanyCode = a.bukrs,
                    Division = a.spart,
                    MaterialBrand = a.matkl
                });

                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
