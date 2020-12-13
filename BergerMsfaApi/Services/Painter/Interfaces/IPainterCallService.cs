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
        #region App
        Task<IEnumerable<PainterCallModel>> AppGetPainterCallListAsync(string employeeId);
        Task<PainterCallModel> AppGetPainterByIdAsync(int Id);
        Task<List<PainterCallModel>> AppGetPainterByPainterIdAsync(string employeeId,int PainterId);
        Task<PainterCallModel> AppCreatePainterCallAsync(int painterCallId);
        Task<PainterCallModel> AppCreatePainterCallAsync(string employeeId,PainterCallModel model);
        Task<PainterCallModel> AppUpdatePainterCallAsync(string employeeId,PainterCallModel model);
      
        #endregion

        #region Portal

        Task<PainterCallModel> GetPainterByIdAsync(int PainterId);
        Task<IEnumerable<PainterCallModel>> GetPainterCallListAsync();
        Task<PainterCallModel> CreatePainterCallAsync(PainterCallModel model);
        Task<PainterCallModel> UpdatePainterCallAsync(PainterCallModel model);
        #endregion

        #region Common
        Task<bool> IsExistAsync(int Id);

        Task<int> DeletePainterCallByIdlAsync(int PainterId);
        #endregion






    }
}
