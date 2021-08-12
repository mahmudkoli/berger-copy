using System.Collections.Generic;
using System.Threading.Tasks;
using Berger.Odata.Model;

namespace Berger.Odata.Services
{
    public interface IColorBankInstallMachine
    {
        Task<IList<ColorBankMachineDataModel>> GetColorBankInstallMachine(string depot, string startDate, string endDate);
    }
}
