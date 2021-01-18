using BergerMsfaApi.Models.ELearning;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.ELearning.Interfaces
{
    public interface IExamService
    {
        Task<IList<UserQuestionAnswerModel>> GetAllExamReportAsync(int pageIndex, int pageSize);
        Task<IList<AppQuestionSetModel>> GetAllQuestionSetAsync();
        Task<AppQuestionSetModel> GetAllQuestionByQuestionSetIdAsync(int id);
        Task<AppQuestionAnswerResultModel> SaveQuestionAnswerAsync(AppQuestionSetModel model);
    }
}
