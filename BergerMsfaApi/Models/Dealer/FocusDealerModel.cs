using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.FocusDealer
{
    public class FocusDealerModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string DealerName { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeId { get; set; }

        public DateTime ValidFrom { get; set; }
       
        public DateTime ValidTo { get; set; }
    }
}
