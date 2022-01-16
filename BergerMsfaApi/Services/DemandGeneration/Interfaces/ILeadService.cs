using Berger.Data.MsfaEntity.DemandGeneration;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.DealerSalesCall;
using BergerMsfaApi.Models.DemandGeneration;
using BergerMsfaApi.Models.FocusDealer;
using BergerMsfaApi.Models.Notification;
using BergerMsfaApi.Models.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.DemandGeneration.Interfaces
{
    public interface ILeadService
    {
        Task<int> AddLeadGenerateAsync(AppSaveLeadGenerationModel model);
        Task<bool> UpdateLeadGenerateAsync(UpdateLeadGenerationModel model);
        Task<int> AddLeadFollowUpAsync(AppSaveLeadFollowUpModel model);
        Task<LeadGenerationModel> GetByIdAsync(int id);
        Task<AppSaveLeadFollowUpModel> GetLeadFollowUpByLeadGenerateIdAsync(int id);
        Task<QueryResultModel<LeadGenerationModel>> GetAllAsync(LeadGenerationDetailsReportSearchModel query);
        Task<IList<AppLeadGenerationModel>> GetAllPendingProjectByUserIdAsync(int userId);
        Task<IList<AppLeadFollowUpNotificationModel>> GetAllTodayFollowUpByUserIdForNotificationAsync(int userId);
        Task<int> DeleteAsync(int id);
        Task<bool> IsExistAsync(int id);
        Task<IList<AppLeadFollowUpNotificationModel>> GetAllTodayFollowUpByUserIdForNotificationAsync();
        Task DeleteImage(DealerImageModel dealerImageModel);
        Task<LeadGenerationModel> GetLeadByIdAsync(int id);
    }
}
