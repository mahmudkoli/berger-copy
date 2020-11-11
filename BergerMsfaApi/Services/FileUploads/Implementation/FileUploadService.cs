using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Berger.Common.Enumerations;
using BergerMsfaApi.Services.FileUploads.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace BergerMsfaApi.Services.FileUploads.Implementation
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _host;

        public FileUploadService(
            IWebHostEnvironment host)
        {
            _host = host;
        }

        public async Task<string> SaveImageAsync(IFormFile file, string fileName, FileUploadCode type)
        {
            string filePath = this.GetFolderPath(type);

            if (!Directory.Exists(Path.Combine(_host.WebRootPath, filePath)))
                Directory.CreateDirectory(Path.Combine(_host.WebRootPath, filePath));

            fileName += Path.GetExtension(file.FileName); 
            filePath = Path.Combine(filePath, fileName);

            using (var stream = new FileStream(Path.Combine(_host.WebRootPath, filePath), FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filePath;
        }

        public async Task<string> SaveImageAsync(IFormFile file, string fileName, FileUploadCode type, int width, int height)
        {
            string filePath = this.GetFolderPath(type);

            if (!Directory.Exists(Path.Combine(_host.WebRootPath, filePath)))
                Directory.CreateDirectory(Path.Combine(_host.WebRootPath, filePath));

            fileName += Path.GetExtension(file.FileName); 
            filePath = Path.Combine(filePath, fileName);

            using (Image image = Image.FromStream(file.OpenReadStream()))
            {
                Image newImage = image;
                if(image.Width > width || image.Height > height)
                {
                    newImage = await ResizeImageAsync(image, width, height);
                }
                newImage.Save(Path.Combine(_host.WebRootPath, filePath));
            }

            return filePath;
        }

        public async Task<string> SaveImageAsync(string base64String, string fileName, FileUploadCode type, int width, int height)
        {
            string filePath = this.GetFolderPath(type);

            if (!Directory.Exists(Path.Combine(_host.WebRootPath, filePath)))
                Directory.CreateDirectory(Path.Combine(_host.WebRootPath, filePath));

            fileName += ".jpg";
            filePath = Path.Combine(filePath, fileName);

            byte[] bytes = Convert.FromBase64String(base64String);
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                Image image = Image.FromStream(ms);
                Image newImage = image;
                if (image.Width > width || image.Height > height)
                {
                    newImage = await ResizeImageAsync(image, width, height);
                }
                newImage.Save(Path.Combine(_host.WebRootPath, filePath));
            }

            return filePath;
        }

        public async Task<Image> ResizeImageAsync(Image image, int width, int height)
        {
            Image newImage = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.DrawImage(image, 0, 0, width, height);
            }
            return newImage;
        }

        public async Task DeleteImageAsync(string filePath)
        {
            if (!string.IsNullOrWhiteSpace(filePath) && File.Exists(Path.Combine(_host.WebRootPath, filePath)))
                File.Delete(Path.Combine(_host.WebRootPath, filePath));
        }

        private string GetFolderPath(FileUploadCode type)
        {
            string filePath = Path.Combine("uploads", "images");
            switch (type)
            {
                case FileUploadCode.DealerOpening:
                    filePath = Path.Combine(filePath, "DealerOpening");
                    break;
                case FileUploadCode.PainterRegistration:
                    filePath = Path.Combine(filePath, "PainterRegistration");
                    break;
                case FileUploadCode.DealerSalesCall:
                    filePath = Path.Combine(filePath, "DealerSalesCall");
                    break;
                default:
                    break;
            }
            return filePath;
        }
    }
}
