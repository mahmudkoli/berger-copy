using System;

namespace Berger.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class IgnoreEntityAttribute : Attribute
    {
    }
}
