using System;
using System.Collections.Generic;
using System.Text;

namespace Berger.Worker.Query
{
    class BrandQuery
    {
        private readonly string baseAddress;
        private readonly string brandAddress;


        public BrandQuery()
        {
            baseAddress = BaseData.BaseAddress;
            brandAddress = "ZVMAT_SAL_APP_CDS/ZVMAT_SAL_APP?$format=json";
        }

        public string GetAllTerritory()
        {
            var query = $"{baseAddress}";
            return query;
        }


        public string GetAllBrand()
        {
            var query = $"{baseAddress}{brandAddress}";
            return query;
        }
    }
}
