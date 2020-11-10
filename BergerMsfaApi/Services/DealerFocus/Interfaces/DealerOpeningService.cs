using AutoMapper;
using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.DealerFocus;
using Berger.Data.MsfaEntity.PainterRegistration;
using BergerMsfaApi.Controllers.DealerFocus;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.PainterRegistration;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.DealerFocus.Implementation;
using BergerMsfaApi.Services.FileUploads.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Attachment = Berger.Data.MsfaEntity.PainterRegistration.Attachment;

namespace BergerMsfaApi.Services.DealerFocus.Interfaces
{
    public class DealerOpeningService : IDealerOpeningService
    {
        private IRepository<DealerOpening> _dealerOcpeningSvc;
        private readonly IFileUploadService _fileUploadSvc;
        private readonly IRepository<Attachment> _attachmentSvc;

        public DealerOpeningService(
            IRepository<DealerOpening> dealerOpeningSvc,
              IFileUploadService fileUploadSvc, IRepository<Attachment> attachmentSvc
            )
        {
            _fileUploadSvc = fileUploadSvc;
            _dealerOcpeningSvc = dealerOpeningSvc;
            _attachmentSvc = attachmentSvc;
            
        }
        public async Task<DealerOpeningModel> CreateDealerOpeningAsync(DealerOpeningModel model,List<IFormFile> attachments)
        {
            var _dealerOpening = model.ToMap<DealerOpeningModel, DealerOpening>();
            var result = await _dealerOcpeningSvc.CreateAsync(_dealerOpening);
            var _dealerOpeningModel = result.ToMap<DealerOpening, DealerOpeningModel>();
            foreach (var attach in attachments)
            {
                var path = await _fileUploadSvc.SaveImageAsync(attach, attach.Name, FileUploadCode.DealerOpening);
                var attachment = await _attachmentSvc.CreateAsync(new Attachment { ParentId = result.Id, Name = attach.FileName, Path = path, Format = Path.GetExtension(attach.FileName), Size = attach.Length, TableName = nameof(DealerOpening) });
                _dealerOpeningModel.Attachments.Add(attachment.ToMap<Attachment,AttachmentModel>());
            }

            return _dealerOpeningModel;
        }

        public async Task<int> DeleteDealerOpeningAsync(int DealerId)
        {
            return await _dealerOcpeningSvc.DeleteAsync(f => f.Id == DealerId);
        }

        public async Task<IEnumerable<DealerOpeningModel>> GetDealerOpeningListAsync()
        {
            var result = await _dealerOcpeningSvc.GetAllAsync();
            var dealerOpeningModel = result.ToMap<DealerOpening, DealerOpeningModel>();
            foreach (var item in dealerOpeningModel.ToList())
            {
                var attachment = await _attachmentSvc.FindAsync(f => f.ParentId == item.Id && f.TableName == nameof(DealerOpening));
                if (attachment != null)
                {
                    var attachmentModel = attachment.ToMap<Attachment, AttachmentModel>();
                    item.Attachments.Add(attachmentModel);
                }

            }
            return dealerOpeningModel;
       
        }

        public async Task<DealerOpeningModel> UpdateDealerOpeningAsync(DealerOpeningModel model, List<IFormFile> attachments)
        {
           
            var dealerOpenig = model.ToMap<DealerOpeningModel, DealerOpening>();
             var result=await _dealerOcpeningSvc.UpdateAsync(dealerOpenig);
            var dealerOpenigModel = result.ToMap<DealerOpening, DealerOpeningModel>();

            var existing = await _attachmentSvc.FindAllAsync(f => f.TableName == nameof(DealerOpening) && f.ParentId == model.Id);
            foreach (var item in existing)
            {
                await _fileUploadSvc.DeleteImageAsync(item.Path);

                await _attachmentSvc.DeleteAsync(f => f.Id == item.Id);

            }
            foreach (var attach in attachments)
            {
                var path = await _fileUploadSvc.SaveImageAsync(attach, attach.FileName, FileUploadCode.DealerOpening);
                var _newAttachment = new Attachment { Path = path, Name = attach.FileName, TableName = nameof(DealerOpening), Format = Path.GetExtension(attach.FileName), Size = 1, ParentId = model.Id };
                var attachment = await _attachmentSvc.CreateAsync(_newAttachment);
                dealerOpenigModel.Attachments.Add(attachment.ToMap<Attachment, AttachmentModel>());
            }


            return dealerOpenigModel;
        }

       public async Task<bool> IsExistAsync(int Id)
        {
            return await _dealerOcpeningSvc.IsExistAsync(f => f.Id==Id);
        }
    }
}
