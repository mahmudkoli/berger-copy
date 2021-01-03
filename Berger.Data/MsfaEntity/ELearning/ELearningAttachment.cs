using Berger.Common.Enumerations;
using Berger.Data.Common;

namespace Berger.Data.MsfaEntity.ELearning
{
    public class ELearningAttachment : AuditableEntity<int>
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }
        public string Format { get; set; }
        public EnumAttachmentType Type { get; set; }
        public int ELearningDocumentId { get; set; }
        public ELearningDocument ELearningDocument { get; set; }

        public ELearningAttachment()
        {

        }

        public ELearningAttachment(string name, string path, long size, string format, EnumAttachmentType type)
        {
            Name = name;
            Path = path;
            Size = size;
            Format = format;
            Type = type;
        }

        public ELearningAttachment(string name, string path, EnumAttachmentType type)
        {
            Name = name;
            Path = path;
            Type = type;
        }

        public ELearningAttachment(string path, EnumAttachmentType type)
        {
            Path = path;
            Type = type;
        }
    }
}
