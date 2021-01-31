using System.Collections.Generic;
using System.Threading.Tasks;
using BergerMsfaApi.Models.Users;
using Microsoft.AspNetCore.Http;

namespace BergerMsfaApi.Services.FileImports.Interfaces
{
    public interface IFileImportService
    {
        Task<(IEnumerable<object> Data, string Message)> ExcelImportUserAsync(IFormFile file);
    }
}
