using System.Threading.Tasks;
using Berger.Common.Enumerations;
using Berger.Common.Model;
using Microsoft.AspNetCore.Mvc;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Blob;
using BergerMsfaApi.Services.Blob;
using BergerMsfaApi.Services.FileUploads.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BergerMsfaApi.Controllers.FileUpload
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : BaseController
    {
        private readonly IBlobService _blobService;
        private readonly IFileUploadService _uploadService;


        public FileUploadController(IBlobService blobService, IFileUploadService uploadService)
        {
            _blobService = blobService;
            _uploadService = uploadService;
        }

        [HttpPost("UploadFile")]
        public async Task<ActionResult> PostFile(IFormFile file)
        {
            var result = await file.GetBytes();

            var uploadContentBlobAsync = await _uploadService.SaveFileAsync(file, file.FileName, FileUploadCode.RegisterPainter);

            return Ok(uploadContentBlobAsync);
        }



    }
}
