using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.ELearning;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.ELearning.Interfaces
{
    public interface IQuestionService
    {
        Task<int> AddAsync(SaveQuestionModel model);
        Task<int> UpdateAsync(SaveQuestionModel model);
        Task<QuestionModel> GetByIdAsync(int id);
        Task<QueryResultModel<QuestionModel>> GetAllAsync(QueryObjectModel query);
        Task<IList<QuestionModel>> GetAllByELearningDocumentIdAsync(int id);
        Task<int> DeleteAsync(int eLearningDocumentId);
    }
}
