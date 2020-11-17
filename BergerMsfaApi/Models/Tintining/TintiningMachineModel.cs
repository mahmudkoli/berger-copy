using Berger.Common.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.Tintining
{
    public class TintiningMachineModel
    {
        public int Id { get; set; }
        public string TerritoryCd { get; set; }
        public int EmployeeId { get; set; }
        public int CompanyId { get; set; }
        public decimal No { get; set; }
        public float Cont { get; set; }
        //public int? NoOfCorrection { get; set; }
        public FormCorrectionMode FormCorrectionMode { get; set; }
    }
}
