using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.Scheme
{
    public class SchemeMasterModel
    {
        public SchemeMasterModel()
        {

            SchemeDetails = new List<SchemeDetailModel>();

        }
        public int Id { get; set; }
        public string SchemeName { get; set; }
        public string Condition { get; set; }

        public List<SchemeDetailModel> SchemeDetails { get; set; }

    }
}
