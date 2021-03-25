using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Common.Constants
{
    public static class ConstantsLeadValue
    {
        public const string ProjectStatusUnderConstruction = "Under Construction";
        public const string ProjectStatusPaintingOngoing = "Painting Ongoing";
        public const string ProjectStatusLeadCompleted = "Lead Completed";
        public const string ProjectStatusLeadCompletedTotalWin = "Total Win";
        public const string ProjectStatusLeadCompletedTotalLoss = "Total Loss";
        public const string ProjectStatusLeadCompletedPartialBusiness = "Partial Business";




        public const string OpeningMailBody = "Dear Concerned, A new dealer open request has been been generated from {0} and got approved from {1}. You are requested to open the new dealer into SAP by using the attached informations";
        public const string OpeningMailSubject = "New dealer open Request.REQUEST ID: {}";

    }

    public static class ConstantsCustomerTypeValue
    {
        public const string CustomerTypeDealer = "Dealer";
        public const string CustomerTypeSubDealer = "Sub-Dealer";
        public const string CustomerTypeCustomer = "Customer";
        public const string CustomerTypeDirectProject = "Direct Project";
    }

    public static class SwappingCompetitionValue
    {
        public const string CompetitorBpbl = "BPBL";
        public const string CompetitorAsianPaints = "Asian Paints";
        public const string CompetitorNerolac = "Nerolac";
        public const string CompetitorElitePaints = "Elite Paints";
        public const string CompetitorNippon = "Nippon";
        public const string CompetitorDulux = "Dulux";
        public const string CompetitorMoonstar = "Moonstar";
        public const string CompetitorOthers = "Others";
    }

    public static class ConstantCompanyValue
    {
        public const string companyAP = "AP";
        public const string companyNippon = "Nippon";
        public const string companyNerolac = "Nerolac";
        public const string companyDulux = "Dulux";
        public const string companyJotun = "Jotun";
        public const string companyElite = "Elite";
        public const string companyMoonstar = "Moonstar";
        public const string companyAlKarim = "Al- Karim";
        public const string companyOthers = "Others";
    }

    public static class ConstantIssuesValue
    {
        public const string IssuePosMaterialShort = "POS Material Short";
        public const string IssueShadeCard = "Shade Card";
        public const string IssueShopSignComplain = "Shop Sign Complain";
        public const string IssueDelivery = "Delivery Issue";
        public const string IssueOthers = "Others";
        public const string IssueDamageProduct = "Damage Product";
        public const string IssueCBMachine = "CB Machine Mantainance";
        public const string IssueProductComplaint = "Product Complaint";
    }


}
