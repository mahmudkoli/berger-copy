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
        Task<PainterModel> CreatePainterAsync(PainterModel model, IFormFile profile, List<IFormFile> attachments);
      
        Task<PainterModel> UpdateAsync(PainterModel model);
        Task<PainterModel> GetPainterByIdAsync(int Id);
        Task<bool> IsExistAsync(int Id);
        Task<int> DeleteAsync(int Id);
        Task<PainterModel> UpdatePainterAsync(int painterId, IFormFile file);
       Task<PainterModel> UpdatePainterAsync(int painterId, IFormFile profile, List<IFormFile> attachments);
    }
}
