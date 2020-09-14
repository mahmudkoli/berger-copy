using System.Collections.Generic;
using System.Threading.Tasks;
using BergerMsfaApi.Models.Examples;
using X.PagedList;

namespace BergerMsfaApi.Services.Interfaces
{
    public interface IExampleService
    {
        Task<IEnumerable<ExampleModel>> GetExamplesAsync();
        Task<IPagedList<ExampleModel>> GetPagedExamplesAsync(int pageNumber, int pageSize);
        Task<IEnumerable<ExampleModel>> GetQueryExamplesAsync();
        Task<ExampleModel> GetExampleAsync(int id);
        Task<ExampleModel> SaveAsync(ExampleModel model);
        Task<ExampleModel> CreateAsync(ExampleModel model);
        Task<ExampleModel> UpdateAsync(ExampleModel model);
        Task<int> DeleteAsync(int id);
        Task<bool> IsCodeExistAsync(string code, int id);
    }
}
