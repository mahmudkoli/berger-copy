using BergerMsfaApi.Models.PainterRegistration;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.PainterRegistration.Interfaces
{
    public interface IPainterRegistrationService
    {
        Task<IEnumerable<PainterModel>> GetPainterListAsync();
        Task<PainterModel> CreatePainterAsync(PainterModel model);
        Task<PainterModel> UpdateAsync(PainterModel model);
        Task<PainterModel> GetPainterByIdAsync(int Id);
        Task<bool> IsExistAsync(int Id);
        Task<int> DeleteAsync(int Id);
        Task<PainterModel> UploadPainterProfile(int painterId, IFormFile file);
    }
}
