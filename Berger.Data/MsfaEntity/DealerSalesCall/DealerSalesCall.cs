using Berger.Common.Enumerations;
using Berger.Data.Common;
using Berger.Data.MsfaEntity.PainterRegistration;
using Berger.Data.MsfaEntity.SAPTables;
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
        public DateTime Date { get; set; }
        public bool IsTargetPromotionCommunicated { get; set; }
        public bool IsTargetCommunicated { get; set; }

        public EnumRatings SecondarySalesRatings { get; set; }
        public string SecondarySalesReasonTitle { get; set; }
        public string SecondarySalesReasonRemarks { get; set; }

        public bool IsOSCommunicated { get; set; }
        public bool IsSlippageCommunicated { get; set; }

        public bool IsPremiumProductCommunicated { get; set; }
        public bool IsPremiumProductLifting { get; set; }
        public EnumProductLifting? PremiumProductLifting { get; set; }
        public string PremiumProductLiftingOthers { get; set; }

        public bool IsCBInstalled { get; set; }
        public bool IsCBProductivityCommunicated { get; set; }

        public bool IsMerchendisingPlanogramFollowed { get; set; }

        public bool IsSubDealerInfluence { get; set; }
        public EnumSubDealerInfluence? SubDealerInfluence { get; set; }

        public bool IsPainterInfluence { get; set; }
        public EnumPainterInfluence? PainterInfluence { get; set; }

        public bool IsShopManProductKnowledgeDiscussed { get; set; }
        public bool IsShopManSalesTechniquesDiscussed { get; set; }
        public bool IsShopManMerchendizingImprovementDiscussed { get; set; }

        public bool IsCompetitionPresence { get; set; }
        public EnumCompetitionPresence? CompetitionPresence { get; set; }
        public bool IsCompetitionBetterThanBPBL { get; set; }
        public string CompetitionBetterThanBPBLRemarks { get; set; }
        public string CompetitionComments { get; set; }
        public int? CompetitionImageId { get; set; }
        public Attachment CompetitionImage { get; set; }
        public IList<DealerCompetitionSales> DealerCompetitionSales { get; set; }

        public bool HasDealerSalesIssue { get; set; }
        public EnumDealerSalesIssue? EnumDealerSalesIssue { get; set; }
        public IList<DealerSalesIssue> DealerSalesIssues { get; set; }

        public EnumSatisfaction DealerSatisfaction { get; set; }
        public string DealerSatisfactionReason { get; set; }
    }
}
