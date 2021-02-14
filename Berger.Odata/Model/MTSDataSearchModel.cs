﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.OData.Edm;

namespace Berger.Odata.Model
{
    public class MTSSearchModel
    {
        public string CustomerNo { get; set; }
        //public string Division { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}