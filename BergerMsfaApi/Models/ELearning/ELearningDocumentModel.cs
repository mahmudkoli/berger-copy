using AutoMapper;
using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.ELearning;
using Berger.Data.MsfaEntity.Setup;
using BergerMsfaApi.Mappings;
using BergerMsfaApi.Models.Setup;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.ELearning
{
    public class ELearningDocumentModel : IMapFrom<ELearningDocument>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public DropdownModel Category { get; set; }
        public string UploadDate { get; set; }
        public Status Status { get; set; }
        public IList<ELearningAttachmentModel> ELearningAttachments { get; set; }
        public string AttachedFileName { get; set; }
        public string AttachedLinkAddress { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ELearningDocument, ELearningDocumentModel>()
                .ForMember(dest => dest.UploadDate, opt => opt.MapFrom(src => src.CreatedTime.ToString("dd-MM-yyyy")))
                .AfterMap((src, dest) => ListToString(src, dest));
            profile.CreateMap<ELearningDocumentModel, ELearningDocument>();
            profile.CreateMap<DropdownDetail, DropdownModel>();
            profile.CreateMap<DropdownModel, DropdownDetail>();
        }

        public void ListToString(ELearningDocument src, ELearningDocumentModel dest)
        {
            if (src.ELearningAttachments != null)
            {
                dest.AttachedFileName = string.Join(", ", src.ELearningAttachments.Where(x => x.Type == EnumAttachmentType.File).Select(x => x.Name));
                dest.AttachedLinkAddress = string.Join(", ", src.ELearningAttachments.Where(x => x.Type == EnumAttachmentType.Link).Select(x => x.Path));
            }
        }
    }

    public class SaveELearningDocumentModel : IMapFrom<ELearningDocument>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public Status Status { get; set; }
        public IList<IFormFile> ELearningAttachmentFiles { get; set; }
        public IList<string> ELearningAttachmentUrls { get; set; }
        public IList<ELearningAttachmentModel> ELearningAttachments { get; set; }
    }
}
