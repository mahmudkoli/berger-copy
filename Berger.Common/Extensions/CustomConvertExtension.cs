using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Common.Extensions
{
    public static class CustomConvertExtension
    {
        public static void NullToEmptyString(object parameterObject)
        {
            if (parameterObject == null) return;

            Type type = parameterObject.GetType();
            foreach (var property in type.GetProperties())
            {
                if (property.PropertyType == typeof(string) && property.GetValue(parameterObject) == null)
                    property.SetValue(parameterObject, string.Empty);
            }
        }
    }
}
