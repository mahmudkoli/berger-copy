using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BergerMsfaApi.Models.Users;
using BergerMsfaApi.Services.FileImports.Interfaces;
using BergerMsfaApi.Services.Users.Interfaces;
using ExcelDataReader;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace BergerMsfaApi.Services.FileImports.Implementation
{
    public class FileImportService : IFileImportService
    {
        private readonly IWebHostEnvironment _host;

        public FileImportService(IWebHostEnvironment host)
        {
            _host = host;
        }

        public async Task<(IEnumerable<object> Data, string Message)> ExcelImportUserAsync(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var dataset = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }
                    });

                    DataTable datatable = dataset.Tables[0];

                    foreach (DataColumn dataColumn in datatable.Columns)
                    {
                        dataColumn.ColumnName = Regex.Replace(dataColumn.ColumnName, @"\s+", "").ToUpper();
                    }

                    //return await _userService.ExcelSaveToDatabaseAsync(datatable);
                    return (null, null);
                }
            }
        }
    }
}
