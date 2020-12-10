
using AutoMapper;
using Berger.Data.MsfaEntity.DealerFocus;
using BergerMsfaApi.Mappings;
using BergerMsfaApi.Models.PainterRegistration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Controllers.DealerFocus
{
    public class DealerOpeningModel:IMapFrom<DealerOpening>
    {
        public DealerOpeningModel()
        {
            DealerOpeningAttachments = new List<DealerOpeningAttachmentModel>();
        }
        public int Id { get; set; }
        public string BusinessArea { get; set; }
        public string SaleOffice { get; set; }
        public string SaleGroup { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
        public string EmployeeId { get; set; }
        public List<DealerOpeningAttachmentModel> DealerOpeningAttachments { get; set; }

        public void Mapping(Profile profile)
        {
            
            profile.CreateMap<DealerOpening, DealerOpeningModel>().ReverseMap();

        }
    }
}
