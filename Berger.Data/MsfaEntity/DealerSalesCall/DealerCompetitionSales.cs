using Berger.Data.Common;
using Berger.Data.MsfaEntity.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Data.MsfaEntity.DealerSalesCall
{
    public class DealerCompetitionSales : Entity<int>
    {
        public int DealerSalesCallId { get; set; }
        public DealerSalesCall DealerSalesCall { get; set; }

        public int CompanyId { get; set; }
        public DropdownDetail Company { get; set; }
        public decimal AverageMonthlySales { get; set; }
        public decimal ActualAMDSales { get; set; }
    }
}
