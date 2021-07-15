using Berger.Odata.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Odata.Services
{
    public interface IStockDataService
    {
        Task<IList<MaterialStockResultModel>> GetMaterialStock(MaterialStockSearchModel model);
    }
}
