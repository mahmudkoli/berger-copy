using System;
using System.Collections.Generic;
using System.Text;

namespace Berger.Worker.Query
{
    class CustomerQuery
    {
        private readonly string baseAddress;
        private readonly string customerAddress;


        public CustomerQuery()
        {
            baseAddress = BaseData.BaseAddress;
            customerAddress = "ZCUSTOMER_MASTER_API_CDS/ZCustomer_Master_API?$format=json";
        }

        public string GetAllTerritory()
        {
            var query = $"{baseAddress}";
            return query;
        }


        public string GetAllCustomer()
        {
            var query = $"{baseAddress}{customerAddress}";
            return query;
        }
    }
}
