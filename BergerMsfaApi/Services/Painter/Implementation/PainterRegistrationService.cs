using AutoMapper;
using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.PainterRegistration;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Painter;
using BergerMsfaApi.Models.PainterRegistration;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.FileUploads.Interfaces;
using BergerMsfaApi.Services.PainterRegistration.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.PainterRegistration.Implementation
{
    public class PainterRegistrationService : IPainterRegistrationService
    {
        private readonly IRepository<Painter> _painterSvc;
        private readonly IRepository<Attachment> _attachmentSvc;
        private readonly IRepository<PainterAttachment> _painterAttachmentSvc;
        private readonly IFileUploadService _fileUploadSvc;
        public PainterRegistrationService(
            IRepository<Painter> painterSvc,
            IRepository<PainterAttachment> painterAttachmentSvc,
        IFileUploadService fileUploadSvc,
             IRepository<Attachment> attachmentSvc
            )
        {
            _painterSvc = painterSvc;
            _fileUploadSvc = fileUploadSvc;
            _attachmentSvc = attachmentSvc;
            _painterAttachmentSvc = painterAttachmentSvc;

        }

        public async Task<PainterModel> CreatePainterAsync(PainterModel model)
        {
            var painter = model.ToMap<PainterModel, Painter>();
            var result = await _painterSvc.CreateAsync(painter);
            return result.ToMap<Painter, PainterModel>();
        }
        public async Task<PainterModel> CreatePainterAsync(PainterModel model, IFormFile profile, List<IFormFile> attachments)
        {
            var _painter = model.ToMap<PainterModel, Painter>();

            var _fileName = $"{_painter.PainterImageUrl}_{_painter.Phone}";
            var _path = await _fileUploadSvc.SaveImageAsync(profile, _fileName, FileUploadCode.PainterRegistration);
            _painter.PainterImageUrl = _path;

            var result = await _painterSvc.CreateAsync(_painter);

            var painterModel = result.ToMap<Painter, PainterModel>();
            foreach (var attach in attachments)
            {
                var path = await _fileUploadSvc.SaveImageAsync(attach, attach.Name, FileUploadCode.PainterRegistration);
                var attachment = await _attachmentSvc.CreateAsync(new Attachment { ParentId = result.Id, Name = attach.FileName, Path = path, Format = Path.GetExtension(attach.FileName), Size = attach.Length, TableName = nameof(Painter) });
                //    painterModel.AttachmentModel.Add(attachment.ToMap<Attachment, AttachmentModel>());
            }
            return painterModel;
        }


        public async Task<PainterModel> GetPainterByIdAsync(int Id)
        {
            var result = await _painterSvc.FindAsync(f => f.Id == Id);
            var painterModel = result.ToMap<Painter, PainterModel>();
            var painterAttachments = await _attachmentSvc.FindAllAsync(f => f.ParentId == painterModel.Id && f.TableName == nameof(Painter));
            //foreach (var attachment in painterAttachments)
            //      painterModel.Attachments.Add(attachment.ToMap<Attachment, AttachmentModel>());

                return painterModel;
        }

        public async Task<IEnumerable<PainterModel>> GetPainterListAsync()
        {
            var result = await _painterSvc.GetAllAsync();
            var painterModel = result.ToMap<Painter, PainterModel>();
            foreach (var item in painterModel.ToList())
            {
                var attachment = await _attachmentSvc.FindAsync(f => f.ParentId == item.Id && f.TableName == nameof(Painter));
                if (attachment != null)
                {
                    var attachmentModel = attachment.ToMap<Attachment, AttachmentModel>();
                    //  item.AttachmentModel.Add(attachmentModel);
                }

            }
            return painterModel;

        }


        public async Task<int> DeleteAsync(int Id)
        {
            if (await _attachmentSvc.AnyAsync((f => f.ParentId == Id && f.TableName == nameof(Painter))))
                await _attachmentSvc.DeleteAsync(f => f.ParentId == Id && f.TableName == nameof(Painter));
            return await _painterSvc.DeleteAsync(f => f.Id == Id);
        }


        public async Task<bool> IsExistAsync(int Id) => await _painterSvc.IsExistAsync(f => f.Id == Id);

        public async Task<PainterModel> UpdateAsync(PainterModel model)
        {
            var painter = model.ToMap<PainterModel, Painter>();
            var result = await _painterSvc.UpdateAsync(painter);
            return result.ToMap<Painter, PainterModel>();
        }

        public async Task<PainterModel> UpdatePainterAsync(int painterId, IFormFile file)
        {
            var _painter = await _painterSvc.FindAsync(f => f.Id == painterId);

            if (_painter.PainterImageUrl != null) await _fileUploadSvc.DeleteImageAsync(_painter.PainterImageUrl);
            if (_painter != null)
            {

                var _fileName = $"{painterId}_{_painter.PainterName}";
                var _path = await _fileUploadSvc.SaveImageAsync(file, _fileName, FileUploadCode.PainterRegistration);
                _painter.PainterImageUrl = _path;

            }
            await _painterSvc.UpdateAsync(_painter);
            return _painter.ToMap<Painter, PainterModel>();
        }

        public async Task<PainterModel> UpdatePainterAsync(int painterId, IFormFile profile, List<IFormFile> attachments)
        {
            var _painter = await _painterSvc.FindIncludeAsync(f => f.Id == painterId);
            if (_painter == null) return null;

            if (_painter.PainterImageUrl != null) await _fileUploadSvc.DeleteImageAsync(_painter.PainterImageUrl);
            var _fileName = $"{painterId}_{_painter.PainterName}";
            _painter.PainterImageUrl = await _fileUploadSvc.SaveImageAsync(profile, _fileName, FileUploadCode.PainterRegistration);

            await _painterSvc.UpdateAsync(_painter);

            var painterModel = _painter.ToMap<Painter, PainterModel>();

            var existing = await _attachmentSvc.FindAllAsync(f => f.TableName == nameof(Painter) && f.ParentId == painterModel.Id);
            foreach (var item in existing)
            {
                await _fileUploadSvc.DeleteImageAsync(item.Path);

                await _attachmentSvc.DeleteAsync(f => f.Id == item.Id);

            }
            foreach (var attach in attachments)
            {
                var path = await _fileUploadSvc.SaveImageAsync(attach, attach.FileName, FileUploadCode.PainterRegistration);
                var _newAttachment = new Attachment { Path = path, Name = attach.FileName, TableName = nameof(Painter), Format = Path.GetExtension(attach.FileName), Size = 1, ParentId = _painter.Id };
                var attachment = await _attachmentSvc.CreateAsync(_newAttachment);
            }
            return painterModel;
        }
        #region App
        public async Task<IEnumerable<PainterModel>> AppGetPainterListAsync()
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PainterAttachmentModel, PainterAttachment>();
                cfg.CreateMap<PainterAttachment, PainterAttachmentModel>();
                cfg.CreateMap<PainterModel, Painter>();
                cfg.CreateMap<Painter, PainterModel>();

            }).CreateMapper();
            var _painters = _painterSvc.GetAllInclude(f => f.Attachments);
            return mapper.Map<List<PainterModel>>(_painters);
        }

    
        public async Task<PainterModel> AppCreatePainterAsync(PainterModel model)
        {
            try
            {
                var mapper = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<PainterAttachmentModel, PainterAttachment>();
                    cfg.CreateMap<PainterAttachment, PainterAttachmentModel>();
                    cfg.CreateMap<PainterModel, Painter>();
                    cfg.CreateMap<Painter, PainterModel>();

                }).CreateMapper();


                var _painter = mapper.Map<Painter>(model);
                var _painterImageFileName = $"{_painter.PainterName}_{_painter.Phone}";
                if (!string.IsNullOrEmpty(_painter.PainterImageUrl)) _painter.PainterImageUrl = await _fileUploadSvc.SaveImageAsync(_painter.PainterImageUrl, _painterImageFileName, FileUploadCode.RegisterPainter, 300, 300);

                foreach (var attach in _painter.Attachments)
                {
                    if (!string.IsNullOrEmpty(attach.Path))
                    {
                        var path = await _fileUploadSvc.SaveImageAsync(attach.Path, attach.Name, FileUploadCode.RegisterPainter, 300, 300);
                        attach.Path = path;
                    }
                }

                var result = await _painterSvc.CreateAsync(_painter);
                return mapper.Map<PainterModel>(result);
            }
            catch (System.Exception ex)
            {

                throw ex;
            }


        }

        public async Task<PainterModel> AppUpdateAsync(PainterModel model)
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PainterAttachmentModel, PainterAttachment>();
                cfg.CreateMap<PainterAttachment, PainterAttachmentModel>();
                cfg.CreateMap<PainterModel, Painter>();
                cfg.CreateMap<Painter, PainterModel>();

            }).CreateMapper();

            var _painter = mapper.Map<Painter>(model);

            var _fileName = $"{model.PainterName}_{model.Phone}";

            var _findPainter = await _painterSvc.FindIncludeAsync(f => f.Id == model.Id, f => f.Attachments);

            if (!string.IsNullOrEmpty(_findPainter.PainterImageUrl))
            {
                await _fileUploadSvc.DeleteImageAsync(_findPainter.PainterImageUrl);

                if (!string.IsNullOrEmpty(_painter.PainterImageUrl))
                    _painter.PainterImageUrl = await _fileUploadSvc.SaveImageAsync(_painter.PainterImageUrl, _fileName, FileUploadCode.RegisterPainter, 300, 300);
            }
            foreach (var item in _findPainter.Attachments)
            {
                await _fileUploadSvc.DeleteImageAsync(item.Path);
                await _painterAttachmentSvc.DeleteAsync(f => f.Id == item.Id);
            }
            var result = await _painterSvc.UpdateAsync(_painter);
            return mapper.Map<PainterModel>(result);
        }

        public async Task<PainterModel> AppGetPainterByIdAsync(int Id)
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PainterAttachmentModel, PainterAttachment>();
                cfg.CreateMap<PainterAttachment, PainterAttachmentModel>();
                cfg.CreateMap<PainterModel, Painter>();
                cfg.CreateMap<Painter, PainterModel>();

            }).CreateMapper();

            var _painter = await _painterSvc.FindIncludeAsync(f => f.Id == Id, f => f.Attachments);
            return mapper.Map<PainterModel>(_painter);

        }

        public async Task<bool> AppDeletePainterByIdAsync(int Id)
        {
            await _painterAttachmentSvc.DeleteAsync(f => f.PainterId == Id);
            return
                await _painterSvc.DeleteAsync(f => f.Id == Id) == 1
                ? true : false;

        }

        public async Task<dynamic> AppGetPainterByPhonesync(string Phone)
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PainterAttachmentModel, PainterAttachment>();
                cfg.CreateMap<PainterAttachment, PainterAttachmentModel>();
                cfg.CreateMap<PainterModel, Painter>();
                cfg.CreateMap<Painter, PainterModel>();

            }).CreateMapper();
          //  sp_GetPainterDataByPhone phone
             var param= new Dictionary<string, object>();
            param.Add("@phone", Phone);
            var result = _painterSvc.DynamicListFromSql("sp_GetPainterDataByPhone", param, true);
            return result;
            //return new PainterModel
            //{
            //    Id = 1,
            //    Phone = Phone,
            //    DepotName= "Dhaka Factory",
            //    TerritroyCd = "Territory 001",
            //    ZoneCd = "Zone 100",
            //    PainterCatId = 1,
            //    AttachedDealerCd = 1,
            //    Loyality=12.0f
            //};
        }

      
        #endregion

    }
}
