using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Berger.Data.MsfaEntity.Target;
using Berger.Odata.Common;
using BergerMsfaApi.Models.KPI;
using BergerMsfaApi.Models.Report;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.KPI.interfaces;
using X.PagedList;

namespace BergerMsfaApi.Services.KPI.Implementation
{
    public class ColorBankInstallationTargetService : IColorBankInstallationTargetService
    {
        private readonly IRepository<ColorBankInstallationTarget> _repository;

        public ColorBankInstallationTargetService(IRepository<ColorBankInstallationTarget> repository)
        {
            _repository = repository;
        }

        public async Task<IList<ColorBankInstallationTargetSaveModel>> GetFyYearData(ColorBankTargetSetupSearchModel query)
        {
            var dbRecords = await _repository.FindByCondition(x => x.Year == query.Year && x.Month >= ConstantsValue.FyYearFirstMonth && x.BusinessArea == query.Depot && query.Territory==x.Territory).ToListAsync();
            var dbRecords2 = await _repository.FindByCondition(x => x.Year == query.Year + 1 && x.Month <= ConstantsValue.FyYearLastMonth && x.BusinessArea == query.Depot && query.Territory == x.Territory).ToListAsync();
            dbRecords.AddRange(dbRecords2);

            Dictionary<int, string> bergerFyMonth = ConstantsValue.GetBergerFyMonth(query.Year);
            var result = new List<ColorBankInstallationTargetSaveModel>();

            foreach (var item in bergerFyMonth)
            {
                var dbSingleRecord = dbRecords.FirstOrDefault(x => x.Month == item.Key);
                result.Add(new ColorBankInstallationTargetSaveModel
                {
                    Territory = dbSingleRecord?.Territory ?? query.Territory,
                    Year = item.Key <= ConstantsValue.FyYearLastMonth ? query.Year + 1 : query.Year,
                    Month = item.Key,
                    MonthName = item.Value,
                    BusinessArea = query.Depot,
                    ColorBankProductivityTarget = dbSingleRecord?.ColorBankProductivityTarget ?? 0,
                    ColorBankInstallTarget = dbSingleRecord?.ColorBankInstallTarget ?? 0,
                    Id = dbSingleRecord?.Id ?? 0
                });
            }
            return result;
        }


        public async Task<int> SaveOrUpdate(IList<ColorBankInstallationTargetSaveModel> model)
        {
            var ids = model.Select(x => x.Id).ToList();

            var dbRecords = await _repository.FindByCondition(x => ids.Contains(x.Id)).ToListAsync();
            int result = 0;
            if (dbRecords.Any())
            {
                foreach (var item in dbRecords)
                {
                    var modelRecord = model.FirstOrDefault(x => x.Id == item.Id);
                    item.Territory = modelRecord.Territory;
                    item.BusinessArea = modelRecord.BusinessArea;
                    item.ColorBankInstallTarget = modelRecord.ColorBankInstallTarget;
                    item.ColorBankProductivityTarget = modelRecord.ColorBankProductivityTarget;
                    item.Month = modelRecord.Month;
                    item.Year = modelRecord.Year;
                }

                var updatedList = await _repository.UpdateListAsync(dbRecords);
                return updatedList.Count;
            }

            else
            {
                var insertList = new List<ColorBankInstallationTarget>();

                foreach (var item in model)
                {
                    insertList.Add(new ColorBankInstallationTarget()
                    {
                        Territory = item.Territory,
                        Year = item.Year,
                        Month = item.Month,
                        BusinessArea = item.BusinessArea,
                        ColorBankInstallTarget = item.ColorBankInstallTarget,
                        ColorBankProductivityTarget = item.ColorBankProductivityTarget
                    });
                }
                var insertedList = await _repository.CreateListAsync(insertList);
                return insertedList.Count;
            }
        }
    }
}
