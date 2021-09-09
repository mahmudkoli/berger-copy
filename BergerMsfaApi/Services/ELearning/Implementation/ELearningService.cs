using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.ELearning;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Common;
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
        private readonly IRepository<ELearningAttachment> _eLearningAttachmentRepository;
        private readonly IFileUploadService _fileUploadService;
        private readonly IMapper _mapper;

        public ELearningService(
                IRepository<ELearningDocument> eLearningDocumentRepository,
                IRepository<ELearningAttachment> eLearningAttachmentRepository,
                IFileUploadService fileUploadService,
                IMapper mapper
            )
        {
            this._eLearningDocumentRepository = eLearningDocumentRepository;
            this._eLearningAttachmentRepository = eLearningAttachmentRepository;
            this._fileUploadService = fileUploadService;
            this._mapper = mapper;
        }

        public async Task<int> AddAsync(SaveELearningDocumentModel model)
        {
            var fileExistingCheck = await IsSameNameFileExistsAsync(model.ELearningAttachmentFiles, model.Id);
            if (fileExistingCheck.IsExists) throw new Exception("File already exists: " + string.Join(", ", fileExistingCheck.ExistsFileNames));

            var eLearningDocument = _mapper.Map<ELearningDocument>(model);

            if(model.ELearningAttachmentFiles != null && model.ELearningAttachmentFiles.Any())
            {
                eLearningDocument.ELearningAttachments = new List<ELearningAttachment>();

                foreach (var item in model.ELearningAttachmentFiles)
                {
                    //var fileName = $"{Path.GetFileNameWithoutExtension(item.FileName)}_{Guid.NewGuid()}";
                    //var attachment = await SaveFileAsync(item, fileName, FileUploadCode.ELearning, EnumAttachmentType.File);
                    var attachment = await SaveFileAsync(item);
                    eLearningDocument.ELearningAttachments.Add(attachment);
                }
            }

            if(model.ELearningAttachmentUrls != null && model.ELearningAttachmentUrls.Any())
            {
                if(eLearningDocument.ELearningAttachments == null)
                    eLearningDocument.ELearningAttachments = new List<ELearningAttachment>();

                foreach (var item in model.ELearningAttachmentUrls)
                {
                    var attachment = new ELearningAttachment(item, EnumAttachmentType.Link);
                    eLearningDocument.ELearningAttachments.Add(attachment);
                }
            }

            eLearningDocument.CreatedTime = DateTime.Now;

            var result = await _eLearningDocumentRepository.CreateAsync(eLearningDocument);
            return result.Id;
        }

        public async Task<QueryResultModel<ELearningDocumentModel>> GetAllAsync(QueryObjectModel query)
        {
            var columnsMap = new Dictionary<string, Expression<Func<ELearningDocument, object>>>()
            {
                ["title"] = v => v.Title,
                ["categoryText"] = v => v.Category.DropdownName,
                ["uploadDate"] = v => v.CreatedTime,
            };

            var result = await _eLearningDocumentRepository.GetAllIncludeAsync(
                                x => x,
                                x => (string.IsNullOrEmpty(query.GlobalSearchValue) || x.Title.Contains(query.GlobalSearchValue) || x.Category.DropdownName.Contains(query.GlobalSearchValue)),
                                x => x.ApplyOrdering(columnsMap, query.SortBy, query.IsSortAscending),
                                x => x.Include(i => i.Category).Include(i => i.ELearningAttachments),
                                query.Page,
                                query.PageSize,
                                true
                            );

            var modelResult = _mapper.Map<IList<ELearningDocumentModel>>(result.Items);

            var queryResult = new QueryResultModel<ELearningDocumentModel>();
            queryResult.Items = modelResult;
            queryResult.TotalFilter = result.TotalFilter;
            queryResult.Total = result.Total;

            return queryResult;
        }

        public async Task<IList<ELearningDocumentModel>> GetAllActiveByCategoryIdAsync(int categoryId)
        {
            var result = await _eLearningDocumentRepository.GetAllIncludeAsync(
                                x => x,
                                x => x.Status == Status.Active && x.CategoryId == categoryId,
                                x => x.OrderByDescending(o => o.CreatedTime),
                                x => x.Include(i => i.Category),
                                true
                            );

            var modelResult = _mapper.Map<IList<ELearningDocumentModel>>(result);

            return modelResult;
        }

        public async Task<IList<ELearningDocumentModel>> GetAllActiveAsync()
        {
            var result = await _eLearningDocumentRepository.GetAllIncludeAsync(
                                x => x,
                                x => x.Status == Status.Active,
                                x => x.OrderByDescending(o => o.CreatedTime),
                                x => x.Include(i => i.Category),
                                true
                            );

            var modelResult = _mapper.Map<IList<ELearningDocumentModel>>(result);

            return modelResult;
        }

        public async Task<ELearningDocumentModel> GetByIdAsync(int id)
        {
            var result = await _eLearningDocumentRepository.GetFirstOrDefaultIncludeAsync(
                                x => x,
                                x => x.Id == id,
                                null,
                                x => x.Include(i => i.ELearningAttachments).Include(i => i.Category),
                                true
                            );

            var modelResult = _mapper.Map<ELearningDocumentModel>(result);

            return modelResult;
        }

        public async Task<int> UpdateAsync(SaveELearningDocumentModel model)
        {
            model.ELearningAttachments ??= new List<ELearningAttachmentModel>();
            var existingAttachFileNames = model.ELearningAttachments?.Select(x => x.Name).ToList();
            var fileExistingCheck = await IsSameNameFileExistsAsync(model.ELearningAttachmentFiles, model.Id, existingAttachFileNames);
            if (fileExistingCheck.IsExists) throw new Exception("File already exists: " + string.Join(", ", fileExistingCheck.ExistsFileNames));

            var eLearningDocument = await _eLearningDocumentRepository.GetFirstOrDefaultIncludeAsync(
                                x => x,
                                x => x.Id == model.Id,
                                null,
                                null,
                                true
                            );

            if (eLearningDocument == null) throw new Exception("ELearning Document not found.");

            eLearningDocument.Title = model.Title;
            eLearningDocument.CategoryId = model.CategoryId;
            eLearningDocument.Status = model.Status;
            eLearningDocument.ModifiedTime = DateTime.Now;

            var result = await _eLearningDocumentRepository.UpdateAsync(eLearningDocument);

            #region delete and update previous attachment
            var previousAttachments = await _eLearningAttachmentRepository.GetAllIncludeAsync(
                                x => x,
                                x => x.ELearningDocumentId == model.Id,
                                null,
                                null,
                                true
                            );

            var updateAttachments = previousAttachments.Where(x => model.ELearningAttachments.Any(y => y.Id == x.Id)).ToList();
            var deleteAttachments = previousAttachments.Except(updateAttachments).ToList();

            foreach (var item in updateAttachments)
            {
                var attachment = model.ELearningAttachments.FirstOrDefault(x => x.Id == item.Id);
                item.Status = attachment.Status;
            }

            if (deleteAttachments.Any())
                await _eLearningAttachmentRepository.DeleteListAsync(deleteAttachments);

            if (updateAttachments.Any())
                await _eLearningAttachmentRepository.UpdateListAsync(updateAttachments);
            #endregion

            #region new attachment added
            var newAttachments = new List<ELearningAttachment>();

            if (model.ELearningAttachmentFiles != null && model.ELearningAttachmentFiles.Any())
            {
                foreach (var item in model.ELearningAttachmentFiles)
                {
                    //var fileName = $"{Path.GetFileNameWithoutExtension(item.FileName)}_{Guid.NewGuid()}";
                    //var attachment = await SaveFileAsync(item, fileName, FileUploadCode.ELearning, EnumAttachmentType.File);
                    var attachment = await SaveFileAsync(item);
                    attachment.ELearningDocumentId = eLearningDocument.Id;
                    newAttachments.Add(attachment);
                }
            }

            if (model.ELearningAttachmentUrls != null && model.ELearningAttachmentUrls.Any())
            {
                foreach (var item in model.ELearningAttachmentUrls)
                {
                    var attachment = new ELearningAttachment(item, EnumAttachmentType.Link);
                    attachment.ELearningDocumentId = eLearningDocument.Id;
                    newAttachments.Add(attachment);
                }
            }

            if(newAttachments.Any())
                await _eLearningAttachmentRepository.CreateListAsync(newAttachments);
            #endregion

            return result.Id;
        }

        public async Task<int> DeleteAsync(int eLearningDocumentId)
        {
            var result = await _eLearningDocumentRepository.DeleteAsync(x => x.Id == eLearningDocumentId);
            return result;
        }

        public async Task<object> GetAllForSelectAsync()
        {
            var result = await _eLearningDocumentRepository.GetAllIncludeAsync(
                                x => new { x.Id, Label = x.Title },
                                x => x.Status == Status.Active,
                                null,
                                null,
                                true
                            );

            return result;
        }

        public async Task<ELearningAttachmentModel> AddAttachmentAsync(int eLearningDocumentId, IFormFile file)
        {
            //var fileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{Guid.NewGuid()}";
            //var attachment = await SaveFileAsync(file, fileName, FileUploadCode.ELearning, EnumAttachmentType.File);
            var attachment = await SaveFileAsync(file);

            var eLearningAttachment = await _eLearningAttachmentRepository.CreateAsync(attachment);

            var result = await _eLearningAttachmentRepository.GetFirstOrDefaultIncludeAsync(
                                x => x,
                                x => x.Id == eLearningAttachment.Id,
                                null,
                                null,
                                true
                            );

            var modelResult = _mapper.Map<ELearningAttachmentModel>(result);

            return modelResult;
        }

        public async Task<ELearningAttachmentModel> AddAttachmentAsync(int eLearningDocumentId, string link)
        {
            var attachment = new ELearningAttachment(link, EnumAttachmentType.Link);

            var eLearningAttachment = await _eLearningAttachmentRepository.CreateAsync(attachment);

            var result = await _eLearningAttachmentRepository.GetFirstOrDefaultIncludeAsync(
                                x => x,
                                x => x.Id == eLearningAttachment.Id,
                                null,
                                null,
                                true
                            );

            var modelResult = _mapper.Map<ELearningAttachmentModel>(result);

            return modelResult;
        }

        public async Task<int> DeleteAttachmentAsync(int eLearningAttachmentId)
        {
            var eLearningAttachment = await _eLearningAttachmentRepository.GetFirstOrDefaultIncludeAsync(
                                x => x,
                                x => x.Id == eLearningAttachmentId,
                                null,
                                null,
                                true
                            );

            var result = await _eLearningAttachmentRepository.DeleteAsync(x => x.Id == eLearningAttachmentId);

            if (eLearningAttachment != null && eLearningAttachment.Type == EnumAttachmentType.File)
                await _fileUploadService.DeleteFileAsync(eLearningAttachment.Path);

            return result;
        }

        private async Task<ELearningAttachment> SaveFileAsync(IFormFile file)
        {
            var fileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{Guid.NewGuid()}";
            var path = await _fileUploadService.SaveFileAsync(file, fileName, FileUploadCode.ELearning);

            var attachment = new ELearningAttachment(file.FileName, path, file.Length, Path.GetExtension(file.FileName), EnumAttachmentType.File);

            return attachment;
        }

        private async Task<(bool IsExists, List<string> ExistsFileNames)> IsSameNameFileExistsAsync(IList<IFormFile> files, int eLearingDocumentId, IList<string> existingAttachFileNames = null)
        {
            var filenames = files == null ? new List<string>() : files.Select(x => x.FileName).ToList();
            var isExists = false;
            var existsFileNames = new List<string>();

            foreach (var fileName in filenames)
            {
                if ((existingAttachFileNames != null && existingAttachFileNames.Contains(fileName)) || await _eLearningAttachmentRepository.IsExistAsync(x => x.ELearningDocumentId != eLearingDocumentId && fileName.ToLower()==x.Name.ToLower()))
                {
                    isExists = true;
                    existsFileNames.Add(fileName);
                }
            }
            return (isExists, existsFileNames);
        }
    }
}
