using System.IO;

namespace BergerMsfaApi.Models.Blob
{
    public class BlobInfo
    {

        public Stream Content { get; set; }
        public string ContentType { get; set; }


        public BlobInfo(Stream content, string contentType)
        {
            Content = content;
            ContentType = contentType;
        }

    }
}
