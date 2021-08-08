using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.KPI
{
    public class NewDealerDevelopmentSaveModel
    {
        public int Id { get; set; }
        public string BusinessArea { get; set; } // Plant, Depot
        public string Territory { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Target { get; set; }
        public int ConversionTarget { get; set; }
        public int NumberofConvertedfromCompetition { get; set; }
    }


    public class SearchNewDealerDevelopment
    {
        public int Year { get; set; }
        public string Territory { get; set; }
        public string Depot { get; set; }
    }
}
