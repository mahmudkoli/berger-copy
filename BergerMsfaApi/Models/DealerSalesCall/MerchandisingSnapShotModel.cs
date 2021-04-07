using AutoMapper;
using Berger.Common.Enumerations;
using Berger.Common.Extensions;
using Berger.Data.MsfaEntity.PainterRegistration;
using Berger.Data.MsfaEntity.SAPTables;
using Berger.Data.MsfaEntity.Setup;
using BergerMsfaApi.Mappings;
using BergerMsfaApi.Models.Dealer;
using BergerMsfaApi.Models.JourneyPlan;
using BergerMsfaApi.Models.PainterRegistration;
using BergerMsfaApi.Models.Setup;
using BergerMsfaApi.Models.Users;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSC = Berger.Data.MsfaEntity.DealerSalesCall;

namespace BergerMsfaApi.Models.MerchandisingSnapShot
{
    public class MerchandisingSnapShotModel : IMapFrom<DSC.MerchandisingSnapShot>
    {
        public int Id { get; set; }
        public int DealerId { get; set; }
        //public DealerInfoModel Dealer { get; set; }
        public string DealerName { get; set; }
        public int UserId { get; set; }
        //public UserInfoModel User { get; set; }
        public string UserFullName { get; set; }
        public DateTime Date { get; set; }
        public string DateText { get; set; }
        public int MerchandisingSnapShotCategoryId { get; set; }
        //public DropdownModel MerchandisingSnapShotCategory { get; set; }
        public string MerchandisingSnapShotCategoryText { get; set; }
        public string OthersSnapShotCategoryName { get; set; }
        public string Remarks { get; set; }
        public string ImageUrl { get; set; }

        public MerchandisingSnapShotModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DSC.MerchandisingSnapShot, MerchandisingSnapShotModel>()
                .ForMember(dest => dest.UserFullName,
                    opt => opt.MapFrom(src => src.User != null ? $"{src.User.FullName}" : string.Empty))
                .ForMember(dest => dest.DealerName,
                    opt => opt.MapFrom(src => src.Dealer != null ? $"{src.Dealer.CustomerName}" : string.Empty))
                .ForMember(dest => dest.Date,
                    opt => opt.MapFrom(src => src.CreatedTime))
                .ForMember(dest => dest.DateText,
                    opt => opt.MapFrom(src => CustomConvertExtension.ObjectToDateString(src.CreatedTime)));
        }
    }

    public class AppMerchandisingSnapShotModel : IMapFrom<DSC.MerchandisingSnapShot>
    {
        public int Id { get; set; }
        public int DealerId { get; set; }
        public DealerInfoModel Dealer { get; set; }
        public int UserId { get; set; }
        public UserInfoModel User { get; set; }
        public string Date { get; set; }
        public int MerchandisingSnapShotCategoryId { get; set; }
        public DropdownModel MerchandisingSnapShotCategory { get; set; }
        public string OthersSnapShotCategoryName { get; set; }
        public string Remarks { get; set; }
        public string ImageUrl { get; set; }

        public AppMerchandisingSnapShotModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DSC.MerchandisingSnapShot, AppMerchandisingSnapShotModel>()
                .ForMember(dest => dest.Date,
                    opt => opt.MapFrom(src => CustomConvertExtension.ObjectToDateString(src.CreatedTime)));
            profile.CreateMap<AppMerchandisingSnapShotModel, DSC.MerchandisingSnapShot>();
            profile.CreateMap<DropdownDetail, DropdownModel>();
            profile.CreateMap<DropdownModel, DropdownDetail>();
        }
    }

    public class SaveMerchandisingSnapShotModel : IMapFrom<DSC.MerchandisingSnapShot>
    {
        public int DealerId { get; set; }
        public int UserId { get; set; }
        public int MerchandisingSnapShotCategoryId { get; set; }
        public string OthersSnapShotCategoryName { get; set; }
        public string Remarks { get; set; }
        public string ImageUrl { get; set; }

        public SaveMerchandisingSnapShotModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DSC.MerchandisingSnapShot, SaveMerchandisingSnapShotModel>();
                //.AddTransform<string>(s => s ?? string.Empty);
            profile.CreateMap<SaveMerchandisingSnapShotModel, DSC.MerchandisingSnapShot>();
        }
    }
}
