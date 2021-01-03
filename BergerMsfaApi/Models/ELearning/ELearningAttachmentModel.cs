using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.ELearning;
using BergerMsfaApi.Mappings;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.ELearning
{
    public class ELearningAttachmentModel : IMapFrom<ELearningAttachment>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }
        public string Format { get; set; }
        public EnumAttachmentType Type { get; set; }
        public Status Status { get; set; }
    }

    public class SaveELearningAttachmentModel
    {
        public int ELearningDocumentId { get; set; }
        public string AttachmentLink { get; set; }
        public IFormFile AttachmentFile { get; set; }
    }
}
