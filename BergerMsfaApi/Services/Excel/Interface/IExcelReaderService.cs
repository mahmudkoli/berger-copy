using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BergerMsfaApi.Services.Excel.Interface
{
    public interface IExcelReaderService
    {
        public Task<DataSet> LoadDataAsync(IFormFile file);
    }
}
