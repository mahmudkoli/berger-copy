using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Berger.Odata.Model;

namespace Berger.Odata.Services
{
    public interface ISyncService
    {
        //Task<IList<SalesDataModel>> GetDailySalesData(DateTime startDate, DateTime endDate);
        Task<IList<MTSDataModel>> GetMonthlyTarget(DateTime fromDate, DateTime toDate);
    }
}
