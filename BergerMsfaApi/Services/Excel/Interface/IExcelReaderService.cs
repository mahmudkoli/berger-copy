using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using BergerMsfaApi.Models.Report;
using Microsoft.AspNetCore.Http;

namespace BergerMsfaApi.Services.Excel.Interface
{
    public interface IExcelReaderService
    {
        public Task<DataSet> LoadDataAsync(IFormFile file);
        public Task<List<T>> LoadDataAsync<T>(IFormFile file) where T : class, new();

        public byte[] WriteToFile<T>(List<T> data);
        public Task<MemoryStream> WriteToFileWithImage(dynamic datas);
        Task<MemoryStream> DealerOpeningWriteToFileWithImage(IList<DealerOpeningReportResultModel> datas);
    }
}
