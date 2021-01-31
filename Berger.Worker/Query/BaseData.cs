using System;
using System.Collections.Generic;
using System.Text;

namespace Berger.Worker.Query
{
    public static class BaseData
    {
        static BaseData()
        {
            //BaseAddress = $"http://bpblecchd.bergerbd.com:8001/sap/opu/odata/sap/";
            BaseAddress = $"http://bpblbgd.bergerbd.com:8000/sap/opu/odata/sap/";
        }
        public static string BaseAddress { get; set; }
    }
}
