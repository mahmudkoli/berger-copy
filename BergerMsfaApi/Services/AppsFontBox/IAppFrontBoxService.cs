using System.Threading.Tasks;
using Berger.Common.Model;
using BergerMsfaApi.Models.AppFontBox;

namespace BergerMsfaApi.Services.AppsFontBox
{
    public interface IAppFrontBoxService
    {
        Task<AppFrontBoxModel> GetAppFontBoxValue(AreaSearchCommonModel area);
    }
}
