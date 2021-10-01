using System.Collections.Generic;
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
        public List<IFormFile> Base64ToImage(List<string> base64Images);
        public bool IsMaxSizeExceded(List<string> base64Images, int sizeMaxMB = 20);
        Task<byte[]> GetFileAsync(string fullPath);
    }
}
