
using AutoMapper;
using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.DealerFocus;
using Berger.Data.MsfaEntity.Users;
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
            dealerOpeningLogs = new List<DealerOpeningLog>();
        }
        public int Id { get; set; }
        public string BusinessArea { get; set; }
        public string SaleOffice { get; set; }
        public string SaleGroup { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
        public string EmployeeId { get; set; }
        public int? CurrentApprovarId { get; set; }
        public int? NextApprovarId { get; set; }
        public UserInfo CurrentApprovar { get; set; }
        public UserInfo NextApprovar { get; set; }
        public string Comment { get; set; }
        public int DealerOpeningStatus { get; set; }
        public string Code { get; set; }

        public List<DealerOpeningAttachmentModel> DealerOpeningAttachments { get; set; }
        public List<DealerOpeningLog> dealerOpeningLogs { get; set; }


        public void Mapping(Profile profile)
        {
            
            profile.CreateMap<DealerOpening, DealerOpeningModel>().ReverseMap();

        }
    }

    public class AppDealerOpeningModel : IMapFrom<DealerOpening>
    {
        public int Id { get; set; }
        public string BusinessArea { get; set; }
        public string SaleOffice { get; set; }
        public string SaleGroup { get; set; }
        public string Territory { get; set; }
        public string Zone { get; set; }
        public int DealerOpeningStatus { get; set; }
        public string DealerOpeningStatusText { get; set; }
        public string Code { get; set; }
        public string Date { get; set; }

        public AppDealerOpeningModel()
        {
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DealerOpening, AppDealerOpeningModel>().ReverseMap();
        }
    }
}
