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
    public interface IQuestionSetService
    {
        Task<int> AddAsync(SaveQuestionSetModel model);
        Task<int> UpdateAsync(SaveQuestionSetModel model);
        Task<QuestionSetModel> GetByIdAsync(int id);
        Task<QueryResultModel<QuestionSetModel>> GetAllAsync(QueryObjectModel query);
        Task<int> DeleteAsync(int eLearningDocumentId);
    }
}
