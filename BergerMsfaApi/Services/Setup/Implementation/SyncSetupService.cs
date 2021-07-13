using System.Collections.Generic;
using System.Threading.Tasks;
using Berger.Data.MsfaEntity.Sync;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.Setup.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BergerMsfaApi.Services.Setup.Implementation
{
    public class SyncSetupService : ISyncSetupService
    {
        private readonly IRepository<SyncSetup> _repository;

        public SyncSetupService(IRepository<SyncSetup> repository)
        {
            _repository = repository;
        }

        public async Task<SyncSetup> GetById(int id)
        {
            return await _repository.FindByCondition(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task Update(SyncSetup setup)
        {
            var model = await _repository.FindByCondition(x => x.Id == setup.Id).FirstOrDefaultAsync();

            if (model != null)
            {
                model.SyncHourlyInterval = setup.SyncHourlyInterval;
               await _repository.UpdateAsync(model);
            }
        }

        public async Task<IEnumerable<SyncSetup>> GetAll()
        {
            return await _repository.GetAllAsync();
        }


    }
}
