using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.PainterRegistration;
using BergerMsfaApi.Models.Report;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;

namespace BergerMsfaApi.Services.PainterRegistration.Interfaces
{
    public interface IPainterRegistrationService
    {


        #region App

        Task<IEnumerable<PainterModel>> AppGetPainterListAsync(string employeeId);
        Task<PainterModel> AppCreatePainterAsync(PainterModel model);
        Task<PainterModel> AppUpdatePainterAsync(PainterModel model);
        Task<PainterModel> AppGetPainterByIdAsync(int Id);
        Task<PainterModel> AppGetPainterByPhonesync(string Phone);
        Task<bool> AppDeletePainterByIdAsync(int Id);



        #endregion

        #region Portal
        Task<QueryResultModel<PainterModel>> GetPainterListAsync(PainterRegistrationReportSearchModel query);
        Task<PainterModel> CreatePainterAsync(PainterModel model);
        Task<PainterModel> CreatePainterAsync(PainterModel model, IFormFile profile, List<IFormFile> attachments);

        Task<PainterModel> UpdateAsync(PainterModel model);
        Task<PainterModel> GetPainterByIdAsync(int Id);
        Task<bool> PainterStatusUpdate(PainterStatusUpdateModel id);

        Task<PainterModel> UpdatePainterAsync(int painterId, IFormFile file);
        Task<PainterModel> UpdatePainterAsync(int painterId, IFormFile profile, List<IFormFile> attachments);

        Task<PainterUpdateModel> GetPainterForEditAsync(int id);
        Task<bool> PainterUpdateAsync(PainterUpdateModel model);
        Task DeleteImage(PainterImageModel models);
        #endregion


        #region Common
        Task<bool> IsExistAsync(int Id);
        Task<int> DeleteAsync(int Id);
        Task<bool> IsExistPainterCallAsync(int id);
        Task<int> DeletePainterCallAsync(int id);
        #endregion
    }
}
