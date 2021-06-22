using BergerMsfaApi.Models.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.Common.Interfaces
{
    public interface IDashboardService
    {
        Task<List<AppDashboardModel>> GetAppDashboardDataAsync();
    }
}
