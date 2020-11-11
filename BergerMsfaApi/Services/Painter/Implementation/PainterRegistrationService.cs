using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.PainterRegistration;
using BergerMsfaApi.Extensions;
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
        private readonly IFileUploadService _fileUploadSvc;
        public PainterRegistrationService(
            IRepository<Painter> painterSvc,
             IFileUploadService fileUploadSvc,
             IRepository<Attachment> attachmentSvc
            )
        {
            _painterSvc = painterSvc;
            _fileUploadSvc = fileUploadSvc;
            _attachmentSvc = attachmentSvc;

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
                // painterModel.AttachmentModel.Add(attachment.ToMap<Attachment, AttachmentModel>());
            }
            return painterModel;
        }

        public async Task<IEnumerable<PainterModel>> AppGetPainterListAsync()
        {
            var _painters = await _painterSvc.GetAllAsync();
            var result = _painters.ToMap<Painter, PainterModel>();
          
            foreach (var item in result.ToList())
            {
                var attachment = await _attachmentSvc.FindAsync(f => f.ParentId == item.Id && f.TableName == nameof(Painter));
                if (attachment != null)
                {
                    //  var attachmentModel = attachment.ToMap<Attachment, AttachmentModel>();
                    item.Attachments.Add(attachment.Path);
                }

            }
            return result;
        }

        public async Task<PainterModel> AppCreatePainterAsync(PainterModel model)
        {
            var _painter = model.ToMap<PainterModel, Painter>();
            var _fileName = $"{_painter.PainterName}_{_painter.Phone}";
            if (!string.IsNullOrEmpty(_painter.PainterImageUrl))
            {
                _painter.PainterImageUrl = await _fileUploadSvc.SaveImageAsync(_painter.PainterImageUrl, _fileName, FileUploadCode.RegisterPainter, 300, 300);
            }
            await _painterSvc.CreateAsync(_painter);
            if (model.Attachments.Count > 0)
            {
                await _attachmentSvc.CreateAsync(new Attachment { Path = _painter.PainterImageUrl, Name = _fileName, TableName = nameof(Painter), ParentId = _painter.Id });
            }
            var result = _painter.ToMap<Painter, PainterModel>();
            return result;
        }

        public async Task<PainterModel> AppUpdateAsync(PainterModel model)
        {
            var _painter = model.ToMap<PainterModel, Painter>();
            var _fileName = $"{model.PainterName}_{model.Phone}";
            var _findPainter = await _painterSvc.FindAsync(f => f.Id == model.Id);
            if (!string.IsNullOrEmpty(_findPainter.PainterImageUrl))
            {
                await _fileUploadSvc.DeleteImageAsync(_findPainter.PainterImageUrl);
                _painter.PainterImageUrl = await _fileUploadSvc.SaveImageAsync(_painter.PainterImageUrl, _fileName, FileUploadCode.RegisterPainter, 300, 300);
            }
            var existing = await _attachmentSvc.FindAllAsync(f => f.TableName == nameof(Painter) && f.ParentId == model.Id);
            foreach (var item in existing)
            {
                await _fileUploadSvc.DeleteImageAsync(item.Path);
                await _attachmentSvc.DeleteAsync(f => f.Id == item.Id && f.TableName == nameof(Painter));
            }
            await _painterSvc.UpdateAsync(_painter);
            var result = _painter.ToMap<Painter, PainterModel>();
            if (model.Attachments.Count > 0)
            {
                var attachment = await _attachmentSvc.CreateAsync(new Attachment { Path = _painter.PainterImageUrl, Name = _fileName, TableName = nameof(Painter), ParentId = _painter.Id });
                result.Attachments.Add(attachment.Path);

            }
            return result;
        }

        public async Task<PainterModel> AppGetPainterByIdAsync(int Id)
        {
            var _painter = await _painterSvc.FindAsync(f => f.Id == Id);
            var result = _painter.ToMap<Painter, PainterModel>();
            if (_painter==null) return result;
            var painterAttachments = await _attachmentSvc.FindAllAsync(f => f.ParentId == _painter.Id && f.TableName == nameof(Painter));
            foreach (var attachment in painterAttachments)
                result.Attachments.Add(attachment.Path);

            return result;
        }

        public async Task<bool> AppDeletePainterByIdAsync(int Id)
        {
            
                await _attachmentSvc.DeleteAsync(f => f.ParentId == Id && f.TableName == nameof(Painter));
            return await _painterSvc.DeleteAsync(f => f.Id == Id) == 1 ? true : false;
            
        }

        public async Task<PainterModel> AppGetPainterByPhonesync(string Phone)
        {
            return new PainterModel
            {
                Id = 1,
                Phone=Phone,
                PainterName = "Painter",
                SaleGroup = "SalesGroup",
                SaleGroupCd = "SalesGroupCd",
                Territroy = "Territroy",
                TerritroyCd = "TerritroyCd",
                Zone="Zone",
                ZoneCd= "ZoneCd",
                PainterCatId= 1,
                PainterCat= "PainterCat",
                AttachedDealer= "AttachedDealer",
                AttachedDealerCd=1


            };
            //var _painter = await _painterSvc.FindAsync(f => f.Phone == Phone);
            //var result = _painter.ToMap<Painter, PainterModel>();
            //var painterAttachments = await _attachmentSvc.FindAllAsync(f => f.ParentId == _painter.Id && f.TableName == nameof(Painter));
            //foreach (var attachment in painterAttachments)
            //    result.Attachments.Add(attachment.Path);

            //return result;
        }
    }
}
