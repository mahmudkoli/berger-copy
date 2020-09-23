using System.Collections.Generic;
using System.Threading.Tasks;
using BergerMsfaApi.Models.Users;
using Microsoft.AspNetCore.Http;

namespace BergerMsfaApi.Services.FileImports.Interfaces
{
    public interface IFileImportService
    {
        Task<(IEnumerable<UserViewModel> Data, string Message)> ExcelImportCAUserAsync(IFormFile file);
    }
}
