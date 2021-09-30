using System.Collections.Generic;
using System.Threading.Tasks;
using Berger.Common.Model;
using BergerMsfaApi.Models.Blob;

namespace BergerMsfaApi.Services.Blob
{
   public interface IBlobService
   {
       public Task<BlobInfo> GetBlobAsync(string name);
       public Task<IEnumerable<string>> ListBlobsAsync(string name);
       public Task<string> UploadFileBlobAsync(string filePath,string fileName);
       public Task<string> UploadContentBlobAsync(string content,string fileName);
       public Task DeleteBlobAsync(string blobName);
       public Task<string> UploadContentBlobAsync(byte[] bytes, string fileName);
   }
}
