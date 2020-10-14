using AutoMapper;
using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity;
using Berger.Data.MsfaEntity.PainterRegistration;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.PainterRegistration;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.FileUploads.Interfaces;
using BergerMsfaApi.Services.PainterRegistration.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.OpenApi.Expressions;
using Microsoft.VisualBasic;
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

            var _fileName = $"{_painter.Name}_{_painter.Phone}";
            var _path = await _fileUploadSvc.SaveImageAsync(profile, _fileName, FileUploadCode.POSMProduct);
            _painter.PainterImage = _path;
            var result = await _painterSvc.CreateAsync(_painter);

            var painterModel = result.ToMap<Painter, PainterModel>();
            foreach (var attach in attachments)
            {
                var path = await _fileUploadSvc.SaveImageAsync(attach, attach.Name, FileUploadCode.POSMProduct);
                var attachment = await _attachmentSvc.CreateAsync(new Attachment { ParentId = result.Id, Name = attach.FileName, Path = path, Format = Path.GetExtension(attach.FileName), Size = attach.Length, TableName = nameof(Painter) });
                painterModel.AttachmentModel.Add(attachment.ToMap<Attachment, AttachmentModel>());
            }
            return painterModel;
        }


        public async Task<PainterModel> GetPainterByIdAsync(int Id)
        {
            var result = await _painterSvc.FindAsync(f => f.Id == Id);
            var painterModel = result.ToMap<Painter, PainterModel>();
            var painterAttachments = await _attachmentSvc.FindAllAsync(f => f.ParentId == painterModel.Id && f.TableName == nameof(Painter));
            foreach (var attachment in painterAttachments)
                painterModel.AttachmentModel.Add(attachment.ToMap<Attachment, AttachmentModel>());

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
                    item.AttachmentModel.Add(attachmentModel);
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

            if (_painter.PainterImage != null) await _fileUploadSvc.DeleteImageAsync(_painter.PainterImage);
            if (_painter != null)
            {
                var _fileName = $"{painterId}_{_painter.Name}";
                var _path = await _fileUploadSvc.SaveImageAsync(file, _fileName, FileUploadCode.POSMProduct);
                _painter.PainterImage = _path;
            }
            await _painterSvc.UpdateAsync(_painter);
            return _painter.ToMap<Painter, PainterModel>();
        }

        public async Task<PainterModel> UpdatePainterAsync(int painterId, IFormFile profile, List<IFormFile> attachments)
        {
            var _painter = await _painterSvc.FindIncludeAsync(f => f.Id == painterId);
            if (_painter == null) return null;

            if (_painter.PainterImage != null) await _fileUploadSvc.DeleteImageAsync(_painter.PainterImage);
            var _fileName = $"{painterId}_{_painter.Name}";
            _painter.PainterImage = await _fileUploadSvc.SaveImageAsync(profile, _fileName, FileUploadCode.POSMProduct);
            await _painterSvc.UpdateAsync(_painter);

            var painterModel = _painter.ToMap<Painter, PainterModel>();

            var existing= await _attachmentSvc.FindAllAsync(f =>  f.TableName == nameof(Painter) && f.ParentId == painterModel.Id);
            foreach (var item in existing)
            {
                await _fileUploadSvc.DeleteImageAsync(item.Path);

                await _attachmentSvc.DeleteAsync(f=>f.Id==item.Id);

            }
            foreach (var attach in attachments)
            {
                var path = await _fileUploadSvc.SaveImageAsync(attach, attach.FileName, FileUploadCode.POSMProduct);
                var _newAttachment = new Attachment { Path = path, Name = attach.FileName, TableName = nameof(Painter), Format = Path.GetExtension(attach.FileName), Size = 1, ParentId = _painter.Id };
                var attachment = await _attachmentSvc.CreateAsync(_newAttachment);
                painterModel.AttachmentModel.Add(attachment.ToMap<Attachment, AttachmentModel>());
            }
            return painterModel;
        }


    }
}
