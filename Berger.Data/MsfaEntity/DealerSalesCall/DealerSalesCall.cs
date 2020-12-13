using Berger.Common.Enumerations;
using Berger.Data.Common;
using Berger.Data.MsfaEntity.PainterRegistration;
using Berger.Data.MsfaEntity.SAPTables;
using Berger.Data.MsfaEntity.Setup;
using Berger.Data.MsfaEntity.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Data.MsfaEntity.DealerSalesCall
{
    public class DealerSalesCall : AuditableEntity<int>
    {
        public int DealerId { get; set; }
        public DealerInfo Dealer { get; set; }
        public int UserId { get; set; }
        public UserInfo User { get; set; }
        public int? JourneyPlanId { get; set; }
        public JourneyPlanMaster JourneyPlan { get; set; }
        //public DateTime Date { get; set; }
        public bool IsTargetPromotionCommunicated { get; set; }
        public bool IsTargetCommunicated { get; set; }

        //public EnumRatings SecondarySalesRatings { get; set; }
        public int SecondarySalesRatingsId { get; set; }
        public DropdownDetail SecondarySalesRatings { get; set; }
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
        public DropdownDetail PremiumProductLifting { get; set; }
        public string PremiumProductLiftingOthers { get; set; }

        public bool IsCBInstalled { get; set; }
        public bool IsCBProductivityCommunicated { get; set; }

        //public bool IsMerchendisingPlanogramFollowed { get; set; }
        public int? MerchendisingId { get; set; }
        public DropdownDetail Merchendising { get; set; }

        public bool HasSubDealerInfluence { get; set; }
        //public EnumSubDealerInfluence? SubDealerInfluence { get; set; }
        public int? SubDealerInfluenceId { get; set; }
        public DropdownDetail SubDealerInfluence { get; set; }

        public bool HasPainterInfluence { get; set; }
        //public EnumPainterInfluence? PainterInfluence { get; set; }
        public int? PainterInfluenceId { get; set; }
        public DropdownDetail PainterInfluence { get; set; }

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
        public string CompetitionProductDisplayImageUrl { get; set; }
        public string CompetitionSchemeModalityComments { get; set; }
        //public int? CompetitionSchemeModalityImageId { get; set; }
        //public Attachment CompetitionSchemeModalityImage { get; set; }
        public string CompetitionSchemeModalityImageUrl { get; set; }
        public string CompetitionShopBoysComments { get; set; }
        public IList<DealerCompetitionSales> DealerCompetitionSales { get; set; }

        public bool HasDealerSalesIssue { get; set; }
        //public EnumDealerSalesIssue? EnumDealerSalesIssue { get; set; }
        public IList<DealerSalesIssue> DealerSalesIssues { get; set; }

        //public EnumSatisfaction DealerSatisfaction { get; set; }
        public int DealerSatisfactionId { get; set; }
        public DropdownDetail DealerSatisfaction { get; set; }
        public string DealerSatisfactionReason { get; set; }

        // for sub dealer
        public bool IsSubDealerCall { get; set; }
        public bool HasBPBLSales { get; set; }
        public decimal BPBLAverageMonthlySales { get; set; }
        public decimal BPBLActualMTDSales { get; set; }
    }
}
