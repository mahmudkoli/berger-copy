﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Berger.Common.Extensions;
using Berger.Odata.Common;
using Berger.Odata.Extensions;

namespace Berger.Odata.Services
{
    public class CollectionDataService : ICollectionDataService
    {
        private readonly IODataService _oDataService;

        public CollectionDataService(IODataService oDataService)
        {
            _oDataService = oDataService;
        }

        public async Task<decimal> GetTotalCollectionValue(IList<int> dealerIds)
        {
            var selectQueryOptionBuilder = new SelectQueryOptionBuilder();

            selectQueryOptionBuilder.AddProperty(DataColumnDef.Collection_Amount);

            var currentDate = DateTime.Now;
            string date = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day).DateTimeFormat();

            var data = await _oDataService.GetCollectionData(selectQueryOptionBuilder, dealerIds, date, date);

            var result = data.Sum(x => CustomConvertExtension.ObjectToDecimal(x.Amount));

            return result;
        }
    }
}