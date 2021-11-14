using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BergerMsfaApi.Extensions;
using Microsoft.Extensions.Configuration;
using BlobInfo = BergerMsfaApi.Models.Blob.BlobInfo;

namespace BergerMsfaApi.Services.Blob
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;
        private readonly string _basePath;
        private IConfiguration _configuration;

        public BlobService(BlobServiceClient blobServiceClient, IConfiguration configuration)
        {
            _blobServiceClient = blobServiceClient;
            _configuration = configuration;
            _containerName = configuration.GetValue<string>("AzureStorageConnectionString:ContainerName");
            _basePath = configuration.GetValue<string>("AzureStorageConnectionString:BasePath");
        }

        public async Task<BlobInfo> GetBlobAsync(string name)
        {
            name = name.Replace(_basePath, "");

            var containerClient = GetBlobContainerClient();

            var blobClient = containerClient.GetBlobClient(name);
            var downloadInfo = await blobClient.DownloadAsync();

            return new BlobInfo(downloadInfo.Value.Content, downloadInfo.Value.ContentType);
        }

        public async Task<IEnumerable<string>> ListBlobsAsync(string name)
        {
            var containerClient = GetBlobContainerClient();
            var items = new List<string>();

            await foreach (var blobItem in containerClient.GetBlobsAsync())
            {
                items.Add(blobItem.Name);
            }

            return items;
        }

        public async Task<string> UploadFileBlobAsync(string filePath, string fileName)
        {
            var blobClient = GetBlobClient(fileName);
            Response<BlobContentInfo> uploadAsync = await blobClient.UploadAsync(filePath, new BlobHttpHeaders
            {
                ContentType = filePath.GetContentType()
            });

            return blobClient.Uri.AbsoluteUri;

        }

        public async Task<string> UploadContentBlobAsync(string content, string fileName)
        {
            var blobClient = GetBlobClient(fileName);
            var bytes = Encoding.UTF8.GetBytes(content);
            await using var memoryStream = new MemoryStream(bytes);
            await blobClient.UploadAsync(memoryStream, new BlobHttpHeaders { ContentType = fileName.GetContentType() });
            return blobClient.Uri.AbsoluteUri;
        }

        public async Task<string> UploadContentBlobAsync(byte[] bytes, string fileName)
        {
            var blobClient = GetBlobClient(fileName);
            await using var memoryStream = new MemoryStream(bytes);
            await blobClient.UploadAsync(memoryStream, new BlobHttpHeaders { ContentType = fileName.GetContentType() });
            return blobClient.Uri.AbsoluteUri;
        }

        public async Task DeleteBlobAsync(string blobName)
        {
            blobName = blobName.Replace(_basePath, "");

            var blobClient = GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync();
        }

        private BlobClient GetBlobClient(string blobName)
        {
            return GetBlobContainerClient().GetBlobClient(blobName);
        }
        private BlobContainerClient GetBlobContainerClient()
        {
            return _blobServiceClient.GetBlobContainerClient(_containerName);
        }

        public async Task<byte[]> GetFileBlobAsync(string blobName)
        {
            blobName = blobName.Replace(_basePath, "");

            var blobClient = GetBlobClient(blobName);

            if (blobClient.ExistsAsync().Result)
            {
                using (var ms = new MemoryStream())
                {
                    blobClient.DownloadTo(ms);
                    return ms.ToArray();
                }
            }
            return new byte[] { };  // returns empty array
        }
    }
}
