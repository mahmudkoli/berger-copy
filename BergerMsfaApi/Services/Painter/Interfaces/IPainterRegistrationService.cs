using BergerMsfaApi.Models.PainterRegistration;
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
        Task<IPagedList<PainterModel>> GetPainterListAsync(int index, int pageSize, string search);
        Task<PainterModel> CreatePainterAsync(PainterModel model);
        Task<PainterModel> CreatePainterAsync(PainterModel model, IFormFile profile, List<IFormFile> attachments);

        Task<PainterModel> UpdateAsync(PainterModel model);
        Task<PainterModel> GetPainterByIdAsync(int Id);
        Task<bool> PainterStatusUpdate(PainterStatusUpdateModel id);

        Task<PainterModel> UpdatePainterAsync(int painterId, IFormFile file);
        Task<PainterModel> UpdatePainterAsync(int painterId, IFormFile profile, List<IFormFile> attachments);
        #endregion


        #region Common
        Task<bool> IsExistAsync(int Id);
        Task<int> DeleteAsync(int Id);
        #endregion
    }
}
