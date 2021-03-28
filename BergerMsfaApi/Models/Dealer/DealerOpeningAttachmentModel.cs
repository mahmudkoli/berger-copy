using AutoMapper;
using Berger.Data.MsfaEntity.DealerFocus;
using BergerMsfaApi.Mappings;
using System.ComponentModel.DataAnnotations;

namespace BergerMsfaApi.Controllers.DealerFocus
{
    public class DealerOpeningAttachmentModel:IMapFrom<DealerOpeningAttachment>
    {
        public string Name { get; set; }
        //[RegularExpression("^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)?$",
        //    ErrorMessage = "path must be properly base64 formatted.")]

        public string Path { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<DealerOpeningAttachment, DealerOpeningAttachmentModel>().ReverseMap();
        }
       
    }
}
