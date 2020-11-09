using BergerMsfaApi.Models.PainterRegistration;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.PainterRegistration.Interfaces
{
    public interface IPainterRegistrationService
    {


        #region App
        Task<IEnumerable<PainterModel>> AppGetPainterListAsync();
        Task<PainterModel> AppCreatePainterAsync(PainterModel model);
        Task<PainterModel> AppUpdateAsync(PainterModel model);
        Task<PainterModel> AppGetPainterByIdAsync(int Id);



        #endregion

        #region Portal
        Task<IEnumerable<PainterModel>> GetPainterListAsync();
        Task<PainterModel> CreatePainterAsync(PainterModel model);
        Task<PainterModel> CreatePainterAsync(PainterModel model, IFormFile profile, List<IFormFile> attachments);

        Task<PainterModel> UpdateAsync(PainterModel model);
        Task<PainterModel> GetPainterByIdAsync(int Id);

        Task<PainterModel> UpdatePainterAsync(int painterId, IFormFile file);
        Task<PainterModel> UpdatePainterAsync(int painterId, IFormFile profile, List<IFormFile> attachments);
        #endregion


        #region Common
        Task<bool> IsExistAsync(int Id);
        Task<int> DeleteAsync(int Id);
        #endregion
    }
}
