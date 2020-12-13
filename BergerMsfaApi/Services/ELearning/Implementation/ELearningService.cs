using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.ELearning;
using BergerMsfaApi.Models.ELearning;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.ELearning.Interfaces;
using BergerMsfaApi.Services.FileUploads.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BergerMsfaApi.Services.ELearning.Implementation
{
    public class ELearningService : IELearningService
    {
        private readonly IRepository<ELearningDocument> _eLearningDocumentRepository;
        private readonly IFileUploadService _fileUploadService;
        private readonly IMapper _mapper;

        public ELearningService(
                IRepository<ELearningDocument> eLearningDocumentRepository,
                IFileUploadService fileUploadService,
                IMapper mapper
            )
        {
            this._eLearningDocumentRepository = eLearningDocumentRepository;
            this._fileUploadService = fileUploadService;
            this._mapper = mapper;
        }

        public async Task<int> AddAsync(SaveELearningDocumentModel model)
        {
            var eLearningDocument = _mapper.Map<ELearningDocument>(model);

            if(model.ELearningAttachmentFiles != null && model.ELearningAttachmentFiles.Any())
            {
                eLearningDocument.ELearningAttachments = new List<ELearningAttachment>();

                foreach (var item in model.ELearningAttachmentFiles)
                {
                    var attachment = await SaveFileAsync(item, item.FileName, FileUploadCode.ELearning, AttachmentType.File);
                    eLearningDocument.ELearningAttachments.Add(attachment);
                }
            }

            if(model.ELearningAttachmentUrls != null && model.ELearningAttachmentUrls.Any())
            {
                if(eLearningDocument.ELearningAttachments == null)
                    eLearningDocument.ELearningAttachments = new List<ELearningAttachment>();

                foreach (var item in model.ELearningAttachmentUrls)
                {
                    var attachment = new ELearningAttachment(item, AttachmentType.File);
                    eLearningDocument.ELearningAttachments.Add(attachment);
                }
            }

            eLearningDocument.CreatedTime = DateTime.Now;

            var result = await _eLearningDocumentRepository.CreateAsync(eLearningDocument);
            return result.Id;
        }

        public async Task<IList<ELearningDocumentModel>> GetAllAsync(int pageIndex, int pageSize)
        {
            var result = await _eLearningDocumentRepository.GetAllIncludeAsync(
                                x => x,
                                null,
                                null,
                                x => x.Include(i => i.Category).Include(i => i.ELearningAttachments),
                                pageIndex,
                                pageSize,
                                true
                            );

            var modelResult = _mapper.Map<IList<ELearningDocumentModel>>(result.Items);

            return modelResult;
        }

        public async Task<ELearningDocumentModel> GetByIdAsync(int id)
        {
            var result = await _eLearningDocumentRepository.GetFirstOrDefaultIncludeAsync(
                                x => x,
                                x => x.Id == id,
                                null,
                                true
                            );

            var modelResult = _mapper.Map<ELearningDocumentModel>(result);

            return modelResult;
        }

        private async Task<ELearningAttachment> SaveFileAsync(IFormFile file, string fileName, FileUploadCode fileUploadCode, AttachmentType attachmentType)
        {
            var path = await _fileUploadService.SaveFileAsync(file, fileName, fileUploadCode);

            var attachment = new ELearningAttachment(fileName, path, file.Length, Path.GetExtension(file.FileName), attachmentType);

            return attachment;
        }
    }
}
