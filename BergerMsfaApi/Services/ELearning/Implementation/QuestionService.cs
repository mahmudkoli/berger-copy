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
    public class QuestionService : IQuestionService
    {
        private readonly IRepository<Question> _questionRepository;
        private readonly IRepository<QuestionOption> _questionOptionRepository;
        private readonly IMapper _mapper;

        public QuestionService(
                IRepository<Question> questionRepository,
                IRepository<QuestionOption> questionOptionRepository,
                IMapper mapper
            )
        {
            this._questionRepository = questionRepository;
            this._questionOptionRepository = questionOptionRepository;
            this._mapper = mapper;
        }

        public async Task<int> AddAsync(SaveQuestionModel model)
        {
            var eLearningDocument = _mapper.Map<Question>(model);

            eLearningDocument.CreatedTime = DateTime.Now;

            var result = await _questionRepository.CreateAsync(eLearningDocument);
            return result.Id;
        }

        public async Task<IList<QuestionModel>> GetAllAsync(int pageIndex, int pageSize)
        {
            var result = await _questionRepository.GetAllIncludeAsync(
                                x => x,
                                null,
                                null,
                                x => x.Include(i => i.ELearningDocument),
                                pageIndex,
                                pageSize,
                                true
                            );

            var modelResult = _mapper.Map<IList<QuestionModel>>(result.Items);

            return modelResult;
        }
        
        public async Task<IList<QuestionModel>> GetAllByELearningDocumentIdAsync(int id)
        {
            var result = await _questionRepository.GetAllIncludeAsync(
                                x => x,
                                x => x.ELearningDocumentId == id,
                                null,
                                null,
                                true
                            );

            var modelResult = _mapper.Map<IList<QuestionModel>>(result);

            return modelResult;
        }

        public async Task<QuestionModel> GetByIdAsync(int id)
        {
            var result = await _questionRepository.GetFirstOrDefaultIncludeAsync(
                                x => x,
                                x => x.Id == id,
                                null,
                                x => x.Include(i => i.ELearningDocument).Include(i => i.QuestionOptions),
                                true
                            );

            var modelResult = _mapper.Map<QuestionModel>(result);

            return modelResult;
        }

        public async Task<int> UpdateAsync(SaveQuestionModel model)
        {
            var eLearningDocument = await _questionRepository.GetFirstOrDefaultIncludeAsync(
                                x => x,
                                x => x.Id == model.Id,
                                null,
                                null,
                                true
                            );

            if (eLearningDocument == null) throw new Exception();

            eLearningDocument.Title = model.Title;
            eLearningDocument.ELearningDocumentId = model.ELearningDocumentId;
            eLearningDocument.Type = model.Type;
            eLearningDocument.Mark = model.Mark;
            eLearningDocument.Status = model.Status;
            eLearningDocument.ModifiedTime = DateTime.Now;

            var result = await _questionRepository.UpdateAsync(eLearningDocument);

            #region delete and update previous attachment
            var previousAttachments = await _questionOptionRepository.GetAllIncludeAsync(
                                x => x,
                                x => x.QuestionId == model.Id,
                                null,
                                null,
                                true
                            );

            var updateAttachments = previousAttachments.Where(x => model.QuestionOptions.Any(y => y.Id == x.Id)).ToList();
            var deleteAttachments = previousAttachments.Except(updateAttachments).ToList();

            foreach (var item in updateAttachments)
            {
                var attachment = model.QuestionOptions.FirstOrDefault(x => x.Id == item.Id);
                item.Title = attachment.Title;
                item.Sequence = attachment.Sequence;
                item.IsCorrectAnswer = attachment.IsCorrectAnswer;
                item.Status = attachment.Status;
            }

            if (deleteAttachments.Any())
                await _questionOptionRepository.DeleteListAsync(deleteAttachments);

            if (updateAttachments.Any())
                await _questionOptionRepository.UpdateListAsync(updateAttachments);
            #endregion

            #region new attachment added
            var newAttachments = model.QuestionOptions.Where(x => x.Id <= 0).ToList();

            foreach (var item in newAttachments)
            {
                item.QuestionId = eLearningDocument.Id;
            }

            var newAttachmentss = _mapper.Map<List<QuestionOption>>(newAttachments);

            if (newAttachmentss.Any())
                await _questionOptionRepository.CreateListAsync(newAttachmentss);
            #endregion

            return result.Id;
        }

        public async Task<int> DeleteAsync(int eLearningDocumentId)
        {
            var result = await _questionRepository.DeleteAsync(x => x.Id == eLearningDocumentId);
            return result;
        }
    }
}
