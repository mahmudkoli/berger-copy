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
    public class ExamService : IExamService
    {
        private readonly IRepository<Question> _questionRepository;
        private readonly IRepository<QuestionOption> _questionOptionRepository;
        private readonly IRepository<QuestionSet> _questionSetRepository;
        private readonly IRepository<QuestionSetCollection> _questionSetCollectionRepository;
        private readonly IRepository<UserQuestionAnswer> _userQuestionAnswerRepository;
        private readonly IRepository<UserQuestionAnswerCollection> _userQuestionAnswerCollectionRepository;
        private readonly IMapper _mapper;

        public ExamService(
                IRepository<Question> questionRepository,
                IRepository<QuestionOption> questionOptionRepository,
                IRepository<QuestionSet> questionSetRepository,
                IRepository<QuestionSetCollection> questionSetCollectionRepository,
                IRepository<UserQuestionAnswer> userQuestionAnswerRepository,
                IRepository<UserQuestionAnswerCollection> userQuestionAnswerCollectionRepository,
                IMapper mapper
            )
        {
            this._questionRepository = questionRepository;
            this._questionOptionRepository = questionOptionRepository;
            this._questionSetRepository = questionSetRepository;
            this._questionSetCollectionRepository = questionSetCollectionRepository;
            this._userQuestionAnswerRepository = userQuestionAnswerRepository;
            this._userQuestionAnswerCollectionRepository = userQuestionAnswerCollectionRepository;
            this._mapper = mapper;
        }

        public async Task<QueryResultModel<UserQuestionAnswerModel>> GetAllExamReportAsync(QueryObjectModel query)
        {
            var columnsMap = new Dictionary<string, Expression<Func<UserQuestionAnswer, object>>>()
            {
                ["userFullName"] = v => v.UserInfo.FullName,
                ["questionSetTitle"] = v => v.QuestionSet.Title,
                ["questionSetLevel"] = v => v.QuestionSet.Level,
                ["totalMark"] = v => v.QuestionSet.TotalMark,
                ["userMark"] = v => v.TotalMark,
                ["passStatus"] = v => v.Passed,
                ["examDate"] = v => v.CreatedTime,
            };

            var result = await _userQuestionAnswerRepository.GetAllIncludeAsync(
                                x => x,
                                x => (string.IsNullOrEmpty(query.GlobalSearchValue) || 
                                x.UserInfo.FullName.Contains(query.GlobalSearchValue) || 
                                x.UserInfo.EmployeeId == query.GlobalSearchValue || 
                                x.QuestionSet.Title.Contains(query.GlobalSearchValue)),
                                x => x.ApplyOrdering(columnsMap, query.SortBy, query.IsSortAscending),
                                x => x.Include(i => i.UserInfo).Include(i => i.QuestionSet),
                                query.Page,
                                query.PageSize,
                                true
                            );

            var modelResult = _mapper.Map<IList<UserQuestionAnswerModel>>(result.Items);

            var queryResult = new QueryResultModel<UserQuestionAnswerModel>();
            queryResult.Items = modelResult;
            queryResult.TotalFilter = result.TotalFilter;
            queryResult.Total = result.Total;

            return queryResult;
        }

        public async Task<IList<AppQuestionSetModel>> GetAllQuestionSetAsync()
        {
            var depots = AppIdentity.AppUser.PlantIdList;
            var datetime = DateTime.Now;

            var result = await _questionSetRepository.GetAllIncludeAsync(
                                x => x,
                                x => x.Status == Status.Active
                                    && (!depots.Any() || !x.QuestionSetDepots.Any() || x.QuestionSetDepots.Any(x => depots.Contains(x.Depot)))
                                    && (datetime.Date >= x.StartDate.Date && datetime.Date <= x.EndDate.Date),
                                x => x.OrderByDescending(o => o.CreatedTime),
                                null,
                                true
                            );

            var modelResult = _mapper.Map<IList<AppQuestionSetModel>>(result);

            return modelResult;
        }

        public async Task<AppQuestionSetModel> GetAllQuestionByQuestionSetIdAsync(int id)
        {
            var questionSet = await _questionSetRepository.GetFirstOrDefaultIncludeAsync(
                                x => x,
                                x => x.Id == id,
                                null,
                                x => x.Include(i => i.QuestionSetCollections)
                                        .ThenInclude(i => i.Question).
                                            ThenInclude(i => i.QuestionOptions),
                                true
                            );

            if (questionSet == null) throw new Exception("Question Set not found.");

            var modelQuestionSet = new AppQuestionSetModel();
                modelQuestionSet.Id = questionSet.Id;
                modelQuestionSet.Title = questionSet.Title;
                modelQuestionSet.Level = questionSet.Level;
                modelQuestionSet.TotalMark = questionSet.TotalMark;
                modelQuestionSet.PassMark = questionSet.PassMark;
                modelQuestionSet.TimeOutMinute = questionSet.TimeOutMinute;
                modelQuestionSet.StartDate = questionSet.StartDate.ToString("dd-MM-yyyy");
                modelQuestionSet.EndDate = questionSet.EndDate.ToString("dd-MM-yyyy");
                modelQuestionSet.Questions = new List<AppQuestionModel>();

            foreach (var qus in questionSet.QuestionSetCollections.Where(x => x.Status == Status.Active))
            {
                var modelQus = new AppQuestionModel();
                    modelQus.Id = qus.QuestionId;
                    modelQus.Title = qus.Question.Title;
                    modelQus.Type = qus.Question.Type;
                    modelQus.Mark = qus.Mark;
                    modelQus.QuestionOptions = new List<AppQuestionOptionModel>();
                    modelQus.Answers = new List<int>();

                foreach (var qusOpt in qus.Question.QuestionOptions.Where(x => x.Status == Status.Active))
                {
                    var modelQusOpt = new AppQuestionOptionModel();
                        modelQusOpt.Id = qusOpt.Id;
                        modelQusOpt.Title = qusOpt.Title;
                        modelQusOpt.Sequence = qusOpt.Sequence;
                        modelQusOpt.IsCorrectAnswer = qusOpt.IsCorrectAnswer;

                    modelQus.QuestionOptions.Add(modelQusOpt);
                    modelQus.QuestionOptions = modelQus.QuestionOptions.OrderBy(x => x.Sequence).ToList();
                }

                modelQuestionSet.Questions.Add(modelQus);
            }

            return modelQuestionSet;
        }

        public async Task<AppQuestionAnswerResultModel> SaveQuestionAnswerAsync(AppQuestionSetModel model)
        {
            var userQusAns = new UserQuestionAnswer();
            userQusAns.QuestionSetId = model.Id;
            userQusAns.UserInfoId = model.UserInfoId;
            userQusAns.QuestionAnswerCollections = new List<UserQuestionAnswerCollection>();
            var totalMark = 0;
            var totalCorrectAnswer = 0;

            foreach (var qus in model.Questions)
            {
                var qusAns = new UserQuestionAnswerCollection();
                //qusAns.QuestionSetId = model.Id;
                qusAns.QuestionId = qus.Id;

                if (qus.Type == EnumQuestionType.SingleChoice)
                {
                    var ansId = qus.Answers.FirstOrDefault();
                    var correctAnsId = qus.QuestionOptions.FirstOrDefault(x => x.IsCorrectAnswer).Id;
                    if(correctAnsId == ansId)
                    {
                        qusAns.Mark = qus.Mark;
                        qusAns.IsCorrectAnswer = true;
                        qusAns.Answer = ansId.ToString();
                        totalMark += qus.Mark;
                        totalCorrectAnswer++;
                    }
                } else if(qus.Type == EnumQuestionType.MultipleChoice)
                {
                    var ansIds = qus.Answers;
                    var correctAnsIds = qus.QuestionOptions.Where(x => x.IsCorrectAnswer).Select(x => x.Id).ToList();
                    if (correctAnsIds.All(x => ansIds.Any(y => y == x)) && ansIds.All(x => correctAnsIds.Any(y => y == x)))
                    {
                        qusAns.Mark = qus.Mark;
                        qusAns.IsCorrectAnswer = true;
                        qusAns.Answer = string.Join(',', ansIds);
                        totalMark += qus.Mark;
                        totalCorrectAnswer++;
                    }
                }
            }

            userQusAns.TotalMark = totalMark;
            userQusAns.TotalCorrectAnswer = totalCorrectAnswer;
            userQusAns.Passed = totalMark >= model.PassMark;

            await _userQuestionAnswerRepository.CreateAsync(userQusAns);

            var qusAnsResultModel = new AppQuestionAnswerResultModel();
            qusAnsResultModel.Id = userQusAns.Id;
            qusAnsResultModel.QuestionSetTitle = model.Title;
            qusAnsResultModel.TotalMark = userQusAns.TotalMark;
            qusAnsResultModel.TotalCorrectAnswer = userQusAns.TotalCorrectAnswer;
            qusAnsResultModel.Passed = userQusAns.Passed;

            return qusAnsResultModel;
        }

        public async Task<IList<AppUserQuestionAnswerModel>> GetAllExamReportByCurrentUserAsync()
        {
            var currentUserId = AppIdentity.AppUser.UserId;

            var result = await _userQuestionAnswerRepository.GetAllIncludeAsync(
                                x => x,
                                x => x.UserInfoId == currentUserId && x.Status == Status.Active,
                                x => x.OrderByDescending(o => o.CreatedTime),
                                x => x.Include(i => i.QuestionSet),
                                true
                            );

            var modelResult = _mapper.Map<IList<AppUserQuestionAnswerModel>>(result);

            return modelResult;
        }
    }
}
