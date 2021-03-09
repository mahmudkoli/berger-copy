using System;
using System.Collections.Generic;
using System.Text;
using Berger.Common.Extensions;
using Microsoft.OData.Edm;

namespace Berger.Odata.Model
{
    public class CollectionHistoryResultModel
    {
        public string CustomerNo { get; internal set; }
        public string CustomerName { get; internal set; }

        public CollectionHistoryResultModel()
        {
            CustomConvertExtension.NullToEmptyString(this);
        }
    }
}
