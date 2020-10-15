using BergerMsfaApi.Models.PainterRegistration;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.PainterRegistration.Interfaces
{
    public interface IPaintCallService
    {
        Task<IEnumerable<PainterCallModel>> GetPainterCallListAsync();
        Task<PainterCallModel> CreatePainterCallAsync(PainterCallModel model);
        Task<PainterCallModel> UpdatePainterCallAsync(PainterCallModel model);
        Task<bool> IsExistAsync(int Id);
        Task<PainterCallModel> GetPainterByIdAsync(int PainterId);
        Task<int> DeletePainterCallByIdlAsync(int PainterId);

    }
}
