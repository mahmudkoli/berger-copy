using System.Collections.Generic;
using Berger.Data.MsfaEntity.Target;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.KPI.interfaces;

namespace BergerMsfaApi.Services.KPI.Implementation
{
    public class ColorBankInstallationTargetService: IColorBankInstallationTargetService
    {
        private readonly IRepository<ColorBankInstallationTarget> _repository;

        public ColorBankInstallationTargetService(IRepository<ColorBankInstallationTarget> repository)
        {
            _repository = repository;
        }

        //public async IList<ColorBankInstallationTarget> GetFyYearData(int year,string depot,string territory)
        //{
        //   await _repository.FindAllAsync(x => x.Year == year && x.BusinessArea == depot && x.Territory == territory);
        //}
    }
}
