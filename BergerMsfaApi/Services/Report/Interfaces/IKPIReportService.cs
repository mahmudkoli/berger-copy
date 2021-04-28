using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.DealerSalesCall;
using BergerMsfaApi.Models.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.Report.Interfaces
{
    public interface IKPIReportService
    {
        Task<IList<StrikeRateKPIReportResultModel>> GetStrikeRateKPIReportAsync(StrikeRateKPIReportSearchModel query);
    }
}
