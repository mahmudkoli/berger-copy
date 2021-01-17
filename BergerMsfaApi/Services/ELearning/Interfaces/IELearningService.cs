using BergerMsfaApi.Models.ELearning;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.ELearning.Interfaces
{
    public interface IELearningService
    {
        Task<int> AddAsync(SaveELearningDocumentModel model);
        Task<int> UpdateAsync(SaveELearningDocumentModel model);
        Task<ELearningDocumentModel> GetByIdAsync(int id);
        Task<IList<ELearningDocumentModel>> GetAllAsync(int pageIndex, int pageSize);
        Task<IList<ELearningDocumentModel>> GetAllActiveByCategoryIdAsync(int categoryId);
        Task<IList<ELearningDocumentModel>> GetAllActiveAsync();
        Task<int> DeleteAsync(int eLearningDocumentId);
        Task<object> GetAllForSelectAsync();
        Task<ELearningAttachmentModel> AddAttachmentAsync(int eLearningDocumentId, IFormFile file);
        Task<ELearningAttachmentModel> AddAttachmentAsync(int eLearningDocumentId, string link);
        Task<int> DeleteAttachmentAsync(int eLearningAttachmentId);
    }
}
