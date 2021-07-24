using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Berger.Common.Extensions
{
    public static class EnumExtension
    {
        public static T ToEnumFromDisplayName<T>(this string value)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();

            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == value)
                    {
                        return (T)field.GetValue(null);
                    }
                }
                else
                {
                    if (field.Name == value)
                        return (T)field.GetValue(null);
                }
            }

            throw new ArgumentOutOfRangeException("name");
        }

        public static string GetEnumDescription(Enum value)
        {
            try
            {
                FieldInfo fi = value.GetType().GetField(value.ToString());

                DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

                if (attributes != null && attributes.Any())
                {
                    return attributes.First().Description;
                }

                return value.ToString();
            }
            catch (Exception e)
            {
                return "Enum type not Found";
            }
        }

        public static List<EnumProperty> GetKeyValues(Type enumType)
        {
            var data = new List<EnumProperty>();
            if (enumType.IsEnum)
            {
                foreach (var e in enumType.GetEnumValues())
                {
                    var val = GetDescription(enumType, e.ToString());
                    data.Add(new EnumProperty { Id = Convert.ToInt32(e), Value = val ?? e.ToString() });
                }
                return data;
            }

            return null;
        }

        private static string GetDescription(Type enumType, string name)
        {
            var filedName = enumType.GetFields().FirstOrDefault(x => x.Name == name);
            if (filedName != null)
            {
                var attr = Attribute.GetCustomAttribute(filedName, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attr != null)
                {
                    return attr.Description;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public class EnumProperty
        {
            public int Id { get; set; }
            public string Value { get; set; }
        }
    }

}

    
