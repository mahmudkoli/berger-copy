﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Models.Setup
{
    public class DropdownModel
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public string TypeCode { get; set; }
        public string DropdownName { get; set; }
        public string Description { get; set; }
        public int Sequence { get; set; }
    }
}
