using Berger.Data.Common;
using Berger.Data.MsfaEntity.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Berger.Data.MsfaEntity.PainterRegistration
{
    public class PainterStatusLog: AuditableEntity<int>
    {
        public int PainterId { get; set; }

        [ForeignKey("PainterId")]
        public Painter Painter { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public UserInfo UserInfo { get; set; }
        public string Reason { get; set; }
    }
}
