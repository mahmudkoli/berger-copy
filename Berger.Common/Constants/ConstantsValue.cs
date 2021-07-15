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




        public const string DealerOpeningMailBody = "Dear Concerned, A new dealer opening request has been generated from {0} and got approved from {1}. You are requested to open the new dealer into SAP by using the attached informations.";
        public const string DealerOpeningMailSubject = "New Dealer Opening Request. REQUEST ID: {0}";


        public const string IssueCategoryMailBody = "Dear Concerned, " +
            "A issue has been generated from {0}. You are requested to see the issues.";
        public const string IssueCategoryMailSubject = "Dealer Sales Call Issue Arrived. Issue Category : {0}";

    }

    public static class ConstantsODataValue
    {
        public const string DistrbutionChannelDealer = "10";
        public const string DivisionDecorative = "10";
        public const string CustomerClassificationExclusive = "01";
        public const string CustomerClassificationNonExclusive = "02";
    }

    public static class ConstantsCustomerTypeValue
    {
        public const string Dealer = "Dealer";
        public const string SubDealer = "Sub-Dealer";
        public const string Customer = "Customer";
        public const string DirectProject = "Direct Project";
    }

    public static class ConstantPaintUsageMTDValue
    {
        public const string BPBL = "BPBL";
        public const string AP = "AP";
        public const string Nerolac = "Nerolac";
        public const string Nippon = "Nippon";
        public const string Elite = "Elite";
        //public const string Dulux = "Dulux";
        //public const string Moonstar = "Moonstar";
        public const string Others = "Others";
    }

    public static class ConstantSwappingCompetitionValue
    {
        public const string AP = "AP";
        public const string Nippon = "Nippon";
        public const string Nerolac = "Nerolac";
        public const string Dulux = "Dulux";
        public const string Jotun = "Jotun";
        public const string Elite = "Elite";
        public const string Moonstar = "Moonstar";
        public const string AlKarim = "Al- Karim";
        public const string Others = "Others";
    }

    public static class ConstantIssuesValue
    {
        public const string POSMaterialShort = "POS Material Short";
        public const string ShadeCard = "Shade Card";
        public const string ShopSignComplain = "Shop Sign Complain";
        public const string DeliveryIssue = "Delivery Issue";
        public const string Others = "Others";
        public const string DamageProduct = "Damage Product";
        public const string CBMachineMantainance = "CB Machine Mantainance";
        public const string ProductComplaint = "Product Complain";
    }

    public static class ConstantSnapShotValue
    {
        public const string CompetitionDisplay = "Competition Display";
        public const string GlowSignBoard = "Glow Sign Board";
        public const string ProductDisplay = "Product Display";
        public const string Scheme = "Scheme";
        public const string Brochure = "Brochure";
        public const string Others = "Others";
    }


    public static class ConstantPlatformValue
    {
        public const string PlatformHeaderName = "Platform";
        public const string AppPlatformHeader = "Mobile";
    }


    public static class ConstantAlertNotificationValue
    {
        public const string OccasiontoCelebrate = "Occasion to Celebrate";
        public const string LeadFollowupReminder = "Lead Followup Reminder";
        public const string ChequeBounceNotification = "Cheque Bounce Notification";
        public const string RPRSNotification = "RPRS Notification";
        public const string FastPayCarryNotification = "Fast Pay & Carry Notification";
        public const string CreditLimitCrossNotifiction  = "Credit Limit Cross Notifiction ";
    }

}
