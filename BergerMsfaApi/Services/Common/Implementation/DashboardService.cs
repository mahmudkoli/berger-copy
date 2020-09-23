using BergerMsfaApi.Services.Common.Interfaces;

namespace BergerMsfaApi.Services.Common.Implementation
{
    public class DashboardService : IDashboardService
    {
      


        //private async Task<IEnumerable<DailyCMTaskReportModel>> GetDailyCMActivitiesReportsByCurrentUserAsync(int pageIndex, int pageSize, string fmUserIdsStr)
        //{
        //    #region Store Procedure
        //    var storeProcedure = "spGetDCMAReports";
        //    var parameters = new List<(string, object, bool)>
        //    {
        //        ("PageIndex", pageIndex, false),
        //        ("PageSize", pageSize , false),
        //        ("SearchText", "" , false),
        //        ("OrderBy", "DCMA.Date desc", false),
        //        ("FMIds", fmUserIdsStr, false),
        //        ("TotalCount", 0, true),
        //        ("FilteredCount", 0, true)
        //    };

        //    var result = _dailyCMActivityRepo.GetDataBySP<DailyCMTaskReportModel>(storeProcedure, parameters);

        //    return result.Items.ToList();
        //    #endregion
        //}

  }
}
