using Berger.Data.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Berger.Data.MsfaEntity.DealerFocus
{
    public class FocusDealer:AuditableEntity<int>
    {
        public int Code { get; set; }
        public string EmployeeId { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName ="Date")]
        public DateTime ValidFrom { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime ValidTo { get; set; }

        //public bool IsFocused()
        //{
        //    return this != null && this.Code > 0 && this.ValidTo != null && this.ValidTo.Date >= DateTime.Now.Date;
        //}
    }
}
