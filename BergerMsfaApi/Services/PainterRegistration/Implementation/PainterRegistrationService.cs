using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.PainterRegistration;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.PainterRegistration;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.FileUploads.Interfaces;
using BergerMsfaApi.Services.PainterRegistration.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.PainterRegistration.Implementation
{
    public class PainterRegistrationService : IPainterRegistrationService
    {
        private readonly IRepository<Painter> _painterSvc;
        private readonly IFileUploadService _fileUploadSvc;
        public PainterRegistrationService(
            IRepository<Painter> painterSvc,
             IFileUploadService fileUploadSvc
            )
        {
            _painterSvc = painterSvc;
            _fileUploadSvc = fileUploadSvc;
        }

        public async Task<PainterModel> CreatePainterAsync(PainterModel model)
        {
            var painter = model.ToMap<PainterModel, Painter>();
            var result = await _painterSvc.CreateAsync(painter);
            return result.ToMap<Painter, PainterModel>();
        }

        public async Task<int> DeleteAsync(int Id) => await _painterSvc.DeleteAsync(f => f.Id == Id);

        public async Task<PainterModel> GetPainterByIdAsync(int Id)
        {
            var result = await _painterSvc.FindAsync(f => f.Id == Id);
            return result.ToMap<Painter, PainterModel>();
        }

        public async Task<IEnumerable<PainterModel>> GetPainterListAsync()
        {
            var result = await _painterSvc.GetAllAsync();
            return result.ToMap<Painter, PainterModel>();
        }

        public async Task<bool> IsExistAsync(int Id) => await _painterSvc.IsExistAsync(f => f.Id == Id);

        public async Task<PainterModel> UpdateAsync(PainterModel model)
        {
            var painter = model.ToMap<PainterModel, Painter>();
            var result = await _painterSvc.UpdateAsync(painter);
            return result.ToMap<Painter, PainterModel>();
        }

        public async Task<PainterModel> UploadPainterProfile(int painterId, IFormFile file)
        {
            var _painter = await _painterSvc.FindAsync(f => f.Id == painterId);
            if(_painter.PainterImage!=null) await _fileUploadSvc.DeleteImageAsync(_painter.PainterImage);
            if (_painter != null) {
                var _fileName = $"{painterId}_{_painter.Name}";
                var _path = await _fileUploadSvc.SaveImageAsync(file, _fileName, FileUploadCode.POSMProduct);
                _painter.PainterImage = _path;
            }
            await _painterSvc.UpdateAsync(_painter);
            return _painter.ToMap<Painter, PainterModel>();
        }
    }
}
