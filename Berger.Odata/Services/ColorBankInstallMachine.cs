using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Berger.Odata.Common;
using Berger.Odata.Extensions;
using Berger.Odata.Model;
using Microsoft.OData.Edm;

namespace Berger.Odata.Services
{
    public class ColorBankInstallMachine : IColorBankInstallMachine
    {
        private readonly IODataService _oDataService;

        public ColorBankInstallMachine(IODataService oDataService)
        {
            _oDataService = oDataService;
        }


        public async Task<IList<ColorBankMachineDataModel>> GetColorBankInstallMachine(string depot,string startDate,string endDate)
        {

            var selectQueryOptionBuilder = new SelectQueryOptionBuilder();
            selectQueryOptionBuilder.AddProperty(ColorBankInstalColumnDef.CustomerNo)
                .AddProperty(ColorBankInstalColumnDef.InstallDate);

            var today = DateTime.Now;

            return await _oDataService.GetColorBankInstallData(selectQueryOptionBuilder,depot, startDate,endDate);
        }



    }
}