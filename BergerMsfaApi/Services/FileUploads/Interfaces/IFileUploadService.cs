using System.Drawing;
using System.Threading.Tasks;
using Berger.Common.Enumerations;
using Microsoft.AspNetCore.Http;

namespace BergerMsfaApi.Services.FileUploads.Interfaces
{
    public interface IFileUploadService
    {
        Task<string> SaveFileAsync(IFormFile file, string fileName, FileUploadCode type);
        Task<string> SaveImageAsync(IFormFile file, string fileName, FileUploadCode type);
        Task<string> SaveImageAsync(IFormFile file, string fileName, FileUploadCode type, int width, int height);
        Task<string> SaveImageAsync(string base64String, string fileName, FileUploadCode type);
        Task<string> SaveImageAsync(string base64String, string fileName, FileUploadCode type, int width, int height);
        Task<Image> ResizeImageAsync(Image image, int width, int height);
        Task DeleteImageAsync(string filePath);
        Task DeleteFileAsync(string filePath);
    }
}
