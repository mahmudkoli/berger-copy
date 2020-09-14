using System.Collections.Generic;
using System.Threading.Tasks;
using BergerMsfaApi.Domain.Examples;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Examples;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.Reports.Interfaces;
using X.PagedList;

namespace BergerMsfaApi.Services.Reports.Implementation
{
    public class POSMReportService : IPOSMReportService
    {
        private readonly IRepository<Example> _example;

        public POSMReportService(IRepository<Example> example)
        {
            _example = example;
        }


        public async Task<ExampleModel> CreateAsync(ExampleModel model)
        {
            var example = model.ToMap<ExampleModel, Example>();
            var result = await _example.CreateAsync(example);
            return result.ToMap<Example, ExampleModel>();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var result = await _example.DeleteAsync(s => s.Id == id);
            return result;

        }

        public async Task<bool> IsCodeExistAsync(string code, int id)
        {
            var result = id <= 0
                ? await _example.IsExistAsync(s => s.Code == code)
                : await _example.IsExistAsync(s => s.Code == code && s.Id != id);

            return result;
        }
        public async Task<ExampleModel> GetExampleAsync(int id)
        {
            var result = await _example.FindAsync(s => s.Id == id);
            return result.ToMap<Example, ExampleModel>();
        }

        public async Task<IEnumerable<ExampleModel>> GetExamplesAsync()
        {
            var result = await _example.GetAllAsync();
            return result.ToMap<Example, ExampleModel>();
        }

        public async Task<IPagedList<ExampleModel>> GetPagedExamplesAsync(int pageNumber, int pageSize)
        {
            var result = await _example.GetAllPagedAsync(pageNumber, pageSize);
            return result.ToMap<Example, ExampleModel>();

        }

        public async Task<IEnumerable<ExampleModel>> GetQueryExamplesAsync()
        {
            var result = await _example.ExecuteQueryAsyc<ExampleModel>("SELECT * FROM Examples");
            return result;
        }

        public async Task<ExampleModel> SaveAsync(ExampleModel model)
        {
            var example = model.ToMap<ExampleModel, Example>();
            var result = await _example.CreateOrUpdateAsync(example);
            return result.ToMap<Example, ExampleModel>();
        }

        public async Task<ExampleModel> UpdateAsync(ExampleModel model)
        {
            var example = model.ToMap<ExampleModel, Example>();
            var result = await _example.UpdateAsync(example);
            return result.ToMap<Example, ExampleModel>();
        }


    }
}
