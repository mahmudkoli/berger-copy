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
    }

    public static class ConstantsCustomerTypeValue
    {
        public const string CustomerTypeDealer = "Dealer";
        public const string CustomerTypeSubDealer = "Sub Dealer";
        public const string CustomerTypeCustomer = "Customer";
        public const string CustomerTypeDirectProject = "Direct Project";
    }
}
