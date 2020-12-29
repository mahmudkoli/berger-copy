using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Berger.Data.MsfaEntity.Master
{
 
    public class CreditControlArea
    {
        [Key]
        [Column(Order =1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CreditControlAreaId { get; set; }
        public string Description { get; set; }
    }
}
