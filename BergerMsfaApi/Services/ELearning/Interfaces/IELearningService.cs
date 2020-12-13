using BergerMsfaApi.Models.ELearning;
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
        Task<ELearningDocumentModel> GetByIdAsync(int id);
        Task<IList<ELearningDocumentModel>> GetAllAsync(int pageIndex, int pageSize);
    }
}
