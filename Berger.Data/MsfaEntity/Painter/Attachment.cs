﻿using Berger.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Berger.Data.MsfaEntity.PainterRegistration
{
    [Table("Attachments")]
    public class Attachment:AuditableEntity<int>
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }
        public string Format { get; set; }
        public string TableName { get; set; }
        public int ParentId { get; set; }
       
        //[ForeignKey("ParentId")]
        //public Painter ParentAttachment { get; set; }



    }




}
