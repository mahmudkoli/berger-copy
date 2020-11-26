using BergerMsfaApi.Models.DemandGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.DemandGeneration.Interfaces
{
    public interface ILeadService
    {
        Task<int> AddLeadGenerateAsync(SaveLeadGenerationModel model);
        Task<int> AddLeadFollowUpAsync(SaveLeadFollowUpModel model);
        Task<LeadGenerationModel> GetByIdAsync(int id);
        Task<LeadFollowUpModel> GetLeadFollowUpByLeadGenerateIdAsync(int id);
        Task<IList<LeadGenerationModel>> GetAllAsync(int pageIndex, int pageSize);
    }
}
