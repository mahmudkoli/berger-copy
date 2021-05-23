using AutoMapper;
using Berger.Common.Enumerations;
using Berger.Common.Extensions;
using Berger.Data.MsfaEntity.DealerSalesCall;
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

namespace BergerMsfaApi.Models.DealerSalesCall
{
    public class DealerSalesCallModel : IMapFrom<DSC.DealerSalesCall>
    {
        public int Id { get; set; }
        public int DealerId { get; set; }
        //public DealerInfoModel Dealer { get; set; }
        public string DealerName { get; set; }
        public int UserId { get; set; }
        //public UserInfoModel User { get; set; }
        public string UserFullName { get; set; }
        public int? JourneyPlanId { get; set; }
        //public JourneyPlanDetailModel JourneyPlan { get; set; }
        public DateTime Date { get; set; }
        public string DateText { get; set; }
        public bool IsTargetPromotionCommunicated { get; set; }
        public bool IsTargetCommunicated { get; set; }

        //public EnumRatings SecondarySalesRatings { get; set; }
        public int SecondarySalesRatingsId { get; set; }
        //public DropdownModel SecondarySalesRatings { get; set; }
        public string SecondarySalesRatingsText { get; set; }
        public string SecondarySalesReasonTitle { get; set; }
        public string SecondarySalesReasonRemarks { get; set; }

        public bool HasOS { get; set; }
        public bool IsOSCommunicated { get; set; }
        public bool HasSlippage { get; set; }
        public bool IsSlippageCommunicated { get; set; }

        public bool IsPremiumProductCommunicated { get; set; }
        public bool IsPremiumProductLifting { get; set; }
        //public EnumProductLifting? PremiumProductLifting { get; set; }
        public int? PremiumProductLiftingId { get; set; }
        //public DropdownModel PremiumProductLifting { get; set; }
        public string PremiumProductLiftingText { get; set; }
        public string PremiumProductLiftingOthers { get; set; }

        public bool IsCBInstalled { get; set; }
        public bool IsCBProductivityCommunicated { get; set; }

        //public bool IsMerchendisingPlanogramFollowed { get; set; }
        public int? MerchendisingId { get; set; }
        //public DropdownModel Merchendising { get; set; }
        public string MerchendisingText { get; set; }

        public bool HasSubDealerInfluence { get; set; }
        //public EnumSubDealerInfluence? SubDealerInfluence { get; set; }
        public int? SubDealerInfluenceId { get; set; }
        //public DropdownModel SubDealerInfluence { get; set; }
        public string SubDealerInfluenceText { get; set; }

        public bool HasPainterInfluence { get; set; }
        //public EnumPainterInfluence? PainterInfluence { get; set; }
        public int? PainterInfluenceId { get; set; }
        //public DropdownModel PainterInfluence { get; set; }
        public string PainterInfluenceText { get; set; }

        public bool IsShopManProductKnowledgeDiscussed { get; set; }
        public bool IsShopManSalesTechniquesDiscussed { get; set; }
        public bool IsShopManMerchendizingImprovementDiscussed { get; set; }

        public bool HasCompetitionPresence { get; set; }
        //public EnumCompetitionPresence? CompetitionPresence { get; set; }
        public bool IsCompetitionServiceBetterThanBPBL { get; set; }
        public string CompetitionServiceBetterThanBPBLRemarks { get; set; }
        public bool IsCompetitionProductDisplayBetterThanBPBL { get; set; }
        public string CompetitionProductDisplayBetterThanBPBLRemarks { get; set; }
        //public int? CompetitionProductDisplayImageId { get; set; }
        //public AttachmentModel CompetitionProductDisplayImage { get; set; }
        public string CompetitionProductDisplayImageUrl { get; set; }
        public string CompetitionSchemeModalityComments { get; set; }
        //public int? CompetitionSchemeModalityImageId { get; set; }
        //public AttachmentModel CompetitionSchemeModalityImage { get; set; }
        public string CompetitionSchemeModalityImageUrl { get; set; }
        public string CompetitionShopBoysComments { get; set; }
        public IList<DealerCompetitionSalesModel> DealerCompetitionSales { get; set; }

        public bool HasDealerSalesIssue { get; set; }
        //public EnumDealerSalesIssue? EnumDealerSalesIssue { get; set; }
        public IList<DealerSalesIssueModel> DealerSalesIssues { get; set; }

        //public EnumSatisfaction DealerSatisfaction { get; set; }
        public int DealerSatisfactionId { get; set; }
        //public DropdownModel DealerSatisfaction { get; set; }
        public string DealerSatisfactionText { get; set; }
        public string DealerSatisfactionReason { get; set; }

        // for sub dealer
        public bool IsSubDealerCall { get; set; }
        public bool HasBPBLSales { get; set; }
        public decimal BPBLAverageMonthlySales { get; set; }
        public decimal BPBLActualMTDSales { get; set; }

        public DealerSalesCallModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DSC.DealerSalesCall, DealerSalesCallModel>()
                .ForMember(dest => dest.UserFullName,
                    opt => opt.MapFrom(src => src.User != null ? $"{src.User.FullName}" : string.Empty))
                .ForMember(dest => dest.DealerName,
                    opt => opt.MapFrom(src => src.Dealer != null ? $"{src.Dealer.CustomerName}" : string.Empty))
                .ForMember(dest => dest.SecondarySalesRatingsText,
                    opt => opt.MapFrom(src => src.SecondarySalesRatings != null ? $"{src.SecondarySalesRatings.DropdownName}" : string.Empty))
                .ForMember(dest => dest.PremiumProductLiftingText,
                    opt => opt.MapFrom(src => src.PremiumProductLifting != null ? $"{src.PremiumProductLifting.DropdownName}" : string.Empty))
                .ForMember(dest => dest.MerchendisingText,
                    opt => opt.MapFrom(src => src.Merchendising != null ? $"{src.Merchendising.DropdownName}" : string.Empty))
                .ForMember(dest => dest.SubDealerInfluenceText,
                    opt => opt.MapFrom(src => src.SubDealerInfluence != null ? $"{src.SubDealerInfluence.DropdownName}" : string.Empty))
                .ForMember(dest => dest.PainterInfluenceText,
                    opt => opt.MapFrom(src => src.PainterInfluence != null ? $"{src.PainterInfluence.DropdownName}" : string.Empty))
                .ForMember(dest => dest.DealerSatisfactionText,
                    opt => opt.MapFrom(src => src.DealerSatisfaction != null ? $"{src.DealerSatisfaction.DropdownName}" : string.Empty))
                .ForMember(dest => dest.Date,
                    opt => opt.MapFrom(src => src.CreatedTime))
                .ForMember(dest => dest.DateText,
                    opt => opt.MapFrom(src => CustomConvertExtension.ObjectToDateString(src.CreatedTime)));
            //    profile.CreateMap<DealerSalesCallModel, DSC.DealerSalesCall>();
            //    profile.CreateMap<DropdownDetail, DropdownModel>();
            //    profile.CreateMap<DropdownModel, DropdownDetail>();
            //    profile.CreateMap<DealerInfo, DealerInfoModel>();
            //    profile.CreateMap<DealerInfoModel, DealerInfo>();
        }
    }

    public class AppDealerSalesCallModel : IMapFrom<DSC.DealerSalesCall>
    {
        public int Id { get; set; }
        public int DealerId { get; set; }
        public DealerInfoModel Dealer { get; set; }
        public int UserId { get; set; }
        public UserInfoModel User { get; set; }
        public int? JourneyPlanId { get; set; }
        //public JourneyPlanDetailModel JourneyPlan { get; set; }
        //public DateTime Date { get; set; }
        public bool IsTargetPromotionCommunicated { get; set; }
        public bool IsTargetCommunicated { get; set; }

        //public EnumRatings SecondarySalesRatings { get; set; }
        public int SecondarySalesRatingsId { get; set; }
        public DropdownModel SecondarySalesRatings { get; set; }
        public string SecondarySalesReasonTitle { get; set; }
        public string SecondarySalesReasonRemarks { get; set; }

        public bool HasOS { get; set; }
        public bool IsOSCommunicated { get; set; }
        public bool HasSlippage { get; set; }
        public bool IsSlippageCommunicated { get; set; }

        public bool IsPremiumProductCommunicated { get; set; }
        public bool IsPremiumProductLifting { get; set; }
        //public EnumProductLifting? PremiumProductLifting { get; set; }
        public int? PremiumProductLiftingId { get; set; }
        public DropdownModel PremiumProductLifting { get; set; }
        public string PremiumProductLiftingOthers { get; set; }

        public bool IsCBInstalled { get; set; }
        public bool IsCBProductivityCommunicated { get; set; }

        //public bool IsMerchendisingPlanogramFollowed { get; set; }
        public int? MerchendisingId { get; set; }
        public DropdownModel Merchendising { get; set; }

        public bool HasSubDealerInfluence { get; set; }
        //public EnumSubDealerInfluence? SubDealerInfluence { get; set; }
        public int? SubDealerInfluenceId { get; set; }
        public DropdownModel SubDealerInfluence { get; set; }

        public bool HasPainterInfluence { get; set; }
        //public EnumPainterInfluence? PainterInfluence { get; set; }
        public int? PainterInfluenceId { get; set; }
        public DropdownModel PainterInfluence { get; set; }

        public bool IsShopManProductKnowledgeDiscussed { get; set; }
        public bool IsShopManSalesTechniquesDiscussed { get; set; }
        public bool IsShopManMerchendizingImprovementDiscussed { get; set; }

        public bool HasCompetitionPresence { get; set; }
        //public EnumCompetitionPresence? CompetitionPresence { get; set; }
        public bool IsCompetitionServiceBetterThanBPBL { get; set; }
        public string CompetitionServiceBetterThanBPBLRemarks { get; set; }
        public bool IsCompetitionProductDisplayBetterThanBPBL { get; set; }
        public string CompetitionProductDisplayBetterThanBPBLRemarks { get; set; }
        //public int? CompetitionProductDisplayImageId { get; set; }
        //public AttachmentModel CompetitionProductDisplayImage { get; set; }
        public string CompetitionProductDisplayImageUrl { get; set; }
        public string CompetitionSchemeModalityComments { get; set; }
        //public int? CompetitionSchemeModalityImageId { get; set; }
        //public AttachmentModel CompetitionSchemeModalityImage { get; set; }
        public string CompetitionSchemeModalityImageUrl { get; set; }
        public string CompetitionShopBoysComments { get; set; }
        public IList<AppDealerCompetitionSalesModel> DealerCompetitionSales { get; set; }

        public bool HasDealerSalesIssue { get; set; }
        //public EnumDealerSalesIssue? EnumDealerSalesIssue { get; set; }
        public IList<AppDealerSalesIssueModel> DealerSalesIssues { get; set; }

        //public EnumSatisfaction DealerSatisfaction { get; set; }
        public int DealerSatisfactionId { get; set; }
        public DropdownModel DealerSatisfaction { get; set; }
        public string DealerSatisfactionReason { get; set; }

        // for sub dealer
        public bool IsSubDealerCall { get; set; }
        public bool HasBPBLSales { get; set; }
        public decimal BPBLAverageMonthlySales { get; set; }
        public decimal BPBLActualMTDSales { get; set; }

        public AppDealerSalesCallModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DSC.DealerSalesCall, AppDealerSalesCallModel>();
            profile.CreateMap<AppDealerSalesCallModel, DSC.DealerSalesCall>();
            profile.CreateMap<DropdownDetail, DropdownModel>();
            profile.CreateMap<DropdownModel, DropdownDetail>();
        }
    }

    public class SaveDealerSalesCallModel : IMapFrom<DSC.DealerSalesCall>
    {
        public int DealerId { get; set; }
        //public DealerInfo Dealer { get; set; }
        public int UserId { get; set; }
        //public UserInfo User { get; set; }
        public int? JourneyPlanId { get; set; }
        //public JourneyPlanMaster JourneyPlan { get; set; }
        //public DateTime Date { get; set; }
        public bool IsTargetPromotionCommunicated { get; set; }
        public bool IsTargetCommunicated { get; set; }

        //public EnumRatings SecondarySalesRatings { get; set; }
        public int SecondarySalesRatingsId { get; set; }
        //public DropdownDetail SecondarySalesRatings { get; set; }
        public string SecondarySalesReasonTitle { get; set; }
        public string SecondarySalesReasonRemarks { get; set; }

        public bool HasOS { get; set; }
        public bool IsOSCommunicated { get; set; }
        public bool HasSlippage { get; set; }
        public bool IsSlippageCommunicated { get; set; }

        public bool IsPremiumProductCommunicated { get; set; }
        public bool IsPremiumProductLifting { get; set; }
        //public EnumProductLifting? PremiumProductLifting { get; set; }
        public int? PremiumProductLiftingId { get; set; }
        //public DropdownDetail PremiumProductLifting { get; set; }
        public string PremiumProductLiftingOthers { get; set; }

        public bool IsCBInstalled { get; set; }
        public bool IsCBProductivityCommunicated { get; set; }

        //public bool IsMerchendisingPlanogramFollowed { get; set; }
        public int? MerchendisingId { get; set; }
        //public DropdownDetail Merchendising { get; set; }

        public bool HasSubDealerInfluence { get; set; }
        //public EnumSubDealerInfluence? SubDealerInfluence { get; set; }
        public int? SubDealerInfluenceId { get; set; }
        //public DropdownDetail SubDealerInfluence { get; set; }
        public string SubDealerInfluenceDropDownName { get; set; }

        public bool HasPainterInfluence { get; set; }
        //public EnumPainterInfluence? PainterInfluence { get; set; }
        public int? PainterInfluenceId { get; set; }
        //public DropdownDetail PainterInfluence { get; set; }
        public string PainterInfluenceDropDownName { get; set; }

        public bool IsShopManProductKnowledgeDiscussed { get; set; }
        public bool IsShopManSalesTechniquesDiscussed { get; set; }
        public bool IsShopManMerchendizingImprovementDiscussed { get; set; }

        public bool HasCompetitionPresence { get; set; }
        //public EnumCompetitionPresence? CompetitionPresence { get; set; }
        public bool IsCompetitionServiceBetterThanBPBL { get; set; }
        public string CompetitionServiceBetterThanBPBLRemarks { get; set; }
        public bool IsCompetitionProductDisplayBetterThanBPBL { get; set; }
        public string CompetitionProductDisplayBetterThanBPBLRemarks { get; set; }
        //public int? CompetitionProductDisplayImageId { get; set; }
        //public Attachment CompetitionProductDisplayImage { get; set; }
        //public IFormFile CompetitionProductDisplayImageFile { get; set; }
        public string CompetitionProductDisplayImageUrl { get; set; }
        public string CompetitionSchemeModalityComments { get; set; }
        //public int? CompetitionSchemeModalityImageId { get; set; }
        //public Attachment CompetitionSchemeModalityImage { get; set; }
        //public IFormFile CompetitionSchemeModalityImageFile { get; set; }
        public string CompetitionSchemeModalityImageUrl { get; set; }
        public string CompetitionShopBoysComments { get; set; }
        public IList<SaveDealerCompetitionSalesModel> DealerCompetitionSales { get; set; }

        public bool HasDealerSalesIssue { get; set; }
        //public EnumDealerSalesIssue? EnumDealerSalesIssue { get; set; }
        public IList<SaveDealerSalesIssueModel> DealerSalesIssues { get; set; }

        //public EnumSatisfaction DealerSatisfaction { get; set; }
        public int DealerSatisfactionId { get; set; }
        //public DropdownDetail DealerSatisfaction { get; set; }
        public string DealerSatisfactionReason { get; set; }

        // for sub dealer
        public bool IsSubDealerCall { get; set; }
        public bool HasBPBLSales { get; set; }
        public decimal BPBLAverageMonthlySales { get; set; }
        public decimal BPBLActualMTDSales { get; set; }

        public SaveDealerSalesCallModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DSC.DealerSalesCall, SaveDealerSalesCallModel>();
                //.AddTransform<string>(s => s ?? string.Empty);
            profile.CreateMap<SaveDealerSalesCallModel, DSC.DealerSalesCall>();
        }
    }
}
