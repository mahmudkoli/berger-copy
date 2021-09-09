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
    public class QuestionSetService : IQuestionSetService
    {
        private readonly IRepository<Question> _questionRepository;
        private readonly IRepository<QuestionOption> _questionOptionRepository;
        private readonly IRepository<QuestionSet> _questionSetRepository;
        private readonly IRepository<QuestionSetCollection> _questionSetCollectionRepository;
        private readonly IRepository<QuestionSetDepot> _questionSetDepotRepository;
        private readonly IMapper _mapper;

        public QuestionSetService(
                IRepository<Question> questionRepository,
                IRepository<QuestionOption> questionOptionRepository,
                IRepository<QuestionSet> questionSetRepository,
                IRepository<QuestionSetCollection> questionSetCollectionRepository,
                IRepository<QuestionSetDepot> questionSetDepotRepository,
                IMapper mapper
            )
        {
            this._questionRepository = questionRepository;
            this._questionOptionRepository = questionOptionRepository;
            this._questionSetRepository = questionSetRepository;
            this._questionSetCollectionRepository = questionSetCollectionRepository;
            this._questionSetDepotRepository = questionSetDepotRepository;
            this._mapper = mapper;
        }

        public async Task<int> AddAsync(SaveQuestionSetModel model)
        {
            var eLearningDocument = _mapper.Map<QuestionSet>(model);

            eLearningDocument.CreatedTime = DateTime.Now;

            var result = await _questionSetRepository.CreateAsync(eLearningDocument);
            return result.Id;
        }

        public async Task<QueryResultModel<QuestionSetModel>> GetAllAsync(QueryObjectModel query)
        {
            var columnsMap = new Dictionary<string, Expression<Func<QuestionSet, object>>>()
            {
                ["title"] = v => v.Title,
                ["level"] = v => v.Level,
                ["totalMark"] = v => v.TotalMark,
            };

            var result = await _questionSetRepository.GetAllIncludeAsync(
                                x => x,
                                x => (string.IsNullOrEmpty(query.GlobalSearchValue) || x.Title.Contains(query.GlobalSearchValue)),
                                x => x.ApplyOrdering(columnsMap, query.SortBy, query.IsSortAscending),
                                null,
                                query.Page,
                                query.PageSize,
                                true
                            );

            var modelResult = _mapper.Map<IList<QuestionSetModel>>(result.Items);

            var queryResult = new QueryResultModel<QuestionSetModel>();
            queryResult.Items = modelResult;
            queryResult.TotalFilter = result.TotalFilter;
            queryResult.Total = result.Total;

            return queryResult;
        }

        public async Task<QuestionSetModel> GetByIdAsync(int id)
        {
            var result = await _questionSetRepository.GetFirstOrDefaultIncludeAsync(
                                x => x,
                                x => x.Id == id,
                                null,
                                x => x.Include(i => i.QuestionSetCollections).Include(i => i.QuestionSetDepots),
                                true
                            );

            var modelResult = _mapper.Map<QuestionSetModel>(result);

            return modelResult;
        }

        public async Task<int> UpdateAsync(SaveQuestionSetModel model)
        {
            var eLearningDocument = await _questionSetRepository.GetFirstOrDefaultIncludeAsync(
                                x => x,
                                x => x.Id == model.Id,
                                null,
                                null,
                                true
                            );

            if (eLearningDocument == null) throw new Exception();

            eLearningDocument.Title = model.Title;
            eLearningDocument.Level = model.Level;
            eLearningDocument.ELearningDocumentId = model.ELearningDocumentId;
            eLearningDocument.TotalMark = model.TotalMark;
            eLearningDocument.PassMark = model.PassMark;
            eLearningDocument.TimeOutMinute = model.TimeOutMinute;
            eLearningDocument.StartDate = model.StartDate;
            eLearningDocument.EndDate = model.EndDate;
            eLearningDocument.Status = model.Status;
            eLearningDocument.ModifiedTime = DateTime.Now;

            var result = await _questionSetRepository.UpdateAsync(eLearningDocument);

            #region delete and update previous attachment
            var previousAttachments = await _questionSetCollectionRepository.GetAllIncludeAsync(
                                x => x,
                                x => x.QuestionSetId == model.Id,
                                null,
                                null,
                                true
                            );

            var updateAttachments = previousAttachments.Where(x => model.QuestionSetCollections.Any(y => y.Id == x.Id)).ToList();
            var deleteAttachments = previousAttachments.Except(updateAttachments).ToList();

            foreach (var item in updateAttachments)
            {
                var attachment = model.QuestionSetCollections.FirstOrDefault(x => x.Id == item.Id);
                item.Mark = attachment.Mark;
                item.Status = attachment.Status;
            }

            if (deleteAttachments.Any())
                await _questionSetCollectionRepository.DeleteListAsync(deleteAttachments);

            if (updateAttachments.Any())
                await _questionSetCollectionRepository.UpdateListAsync(updateAttachments);
            #endregion

            #region new attachment added
            var newAttachments = model.QuestionSetCollections.Where(x => x.Id <= 0).ToList();

            foreach (var item in newAttachments)
            {
                item.QuestionSetId = eLearningDocument.Id;
            }

            var newAttachmentss = _mapper.Map<List<QuestionSetCollection>>(newAttachments);

            if (newAttachmentss.Any())
                await _questionSetCollectionRepository.CreateListAsync(newAttachmentss);
            #endregion

            #region delete and add previous depots
            var previousDepots = (await _questionSetDepotRepository.GetAllIncludeAsync(
                                x => x,
                                x => x.QuestionSetId == model.Id,
                                null,
                                null,
                                true
                            )).ToList();

            var newDepots = model.Depots.Select(x => new QuestionSetDepot { QuestionSetId = model.Id, Depot = x }).ToList();

            if(!(previousDepots.All(x => newDepots.Any(y => y.Depot == x.Depot)) && newDepots.All(x => previousDepots.Any(y => x.Depot == y.Depot))))
            {
                if (newDepots.Any())
                    await _questionSetDepotRepository.CreateListAsync(newDepots);

                if (previousDepots.Any())
                    await _questionSetDepotRepository.DeleteListAsync(previousDepots);
            }
            #endregion

            return result.Id;
        }

        public async Task<int> DeleteAsync(int eLearningDocumentId)
        {
            var result = await _questionSetRepository.DeleteAsync(x => x.Id == eLearningDocumentId);
            return result;
        }
    }
}
