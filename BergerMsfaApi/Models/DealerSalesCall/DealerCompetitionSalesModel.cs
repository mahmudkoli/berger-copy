using AutoMapper;
using Berger.Common.Extensions;
using Berger.Data.MsfaEntity.DealerSalesCall;
using Berger.Data.MsfaEntity.Setup;
using BergerMsfaApi.Mappings;
using BergerMsfaApi.Models.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.DealerSalesCall
{
    public class DealerCompetitionSalesModel : IMapFrom<DealerCompetitionSales>
    {
        public int Id { get; set; }
        public int DealerSalesCallId { get; set; }
        //public DealerSalesCallModel DealerSalesCall { get; set; }

        public int CompanyId { get; set; }
        //public DropdownModel Company { get; set; }
        public string CompanyName { get; set; }
        public decimal AverageMonthlySales { get; set; }
        public decimal ActualMTDSales { get; set; }

        public DealerCompetitionSalesModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DealerCompetitionSales, DealerCompetitionSalesModel>()
                .ForMember(dest => dest.CompanyName,
                    opt => opt.MapFrom(src => src.Company != null ? $"{src.Company.DropdownName}" : string.Empty));
            //profile.CreateMap<DealerCompetitionSalesModel, DealerCompetitionSales>();
            //profile.CreateMap<DropdownDetail, DropdownModel>();
            //profile.CreateMap<DropdownModel, DropdownDetail>();
        }
    }

    public class AppDealerCompetitionSalesModel : IMapFrom<DealerCompetitionSales>
    {
        public int Id { get; set; }
        public int DealerSalesCallId { get; set; }
        //public DealerSalesCallModel DealerSalesCall { get; set; }

        public int CompanyId { get; set; }
        public DropdownModel Company { get; set; }
        public decimal AverageMonthlySales { get; set; }
        public decimal ActualMTDSales { get; set; }

        public AppDealerCompetitionSalesModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DealerCompetitionSales, AppDealerCompetitionSalesModel>();
            profile.CreateMap<AppDealerCompetitionSalesModel, DealerCompetitionSales>();
            profile.CreateMap<DropdownDetail, DropdownModel>();
            profile.CreateMap<DropdownModel, DropdownDetail>();
        }
    }

    public class SaveDealerCompetitionSalesModel : IMapFrom<DealerCompetitionSales>
    {
        //public int DealerSalesCallId { get; set; }
        //public DealerSalesCallModel DealerSalesCall { get; set; }

        public int CompanyId { get; set; }
        //public DropdownModel Company { get; set; }
        public string CompanyName { get; set; }
        public decimal AverageMonthlySales { get; set; }
        public decimal ActualMTDSales { get; set; }

        public SaveDealerCompetitionSalesModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DealerCompetitionSales, SaveDealerCompetitionSalesModel>();
            profile.CreateMap<SaveDealerCompetitionSalesModel, DealerCompetitionSales>();
        }
    }
}
