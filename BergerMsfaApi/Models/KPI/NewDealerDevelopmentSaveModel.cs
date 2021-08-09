using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.KPI
{
    public class NewDealerDevelopmentSaveModel
    {
        public int Id { get; set; }
        public string MonthName { get; set; }
        public int Actual { get; set; }
        public int ConversionTarget { get; set; }
        public int NumberofConvertedfromCompetition { get; set; }
    }


    public class SearchNewDealerDevelopment
    {
        public int Year { get; set; }
        public string Territory { get; set; }
        public string Depot { get; set; }
    }


    public class NewDealerDevelopmentModel
    {
        
        public string MonthName { get; set; }
        //public int Month { get; set; }
        public int Target { get; set; }
        public int Actual { get; set; }
        public decimal TargetAch { get; set; }
    }


    public class DealerConversionModel
    {

        public string MonthName { get; set; }
        //public int Month { get; set; }
        public int ConversionTarget { get; set; }
        public int NumberofConvertedfromCompetition { get; set; }
    }
}
