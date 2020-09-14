using System;

namespace BergerMsfaApi.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class IgnoreEntityAttribute : Attribute
    {
    }
}
