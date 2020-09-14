using System;

namespace BergerMsfaApi.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public sealed class IgnoreUpdateAttribute : Attribute
    {
    }
}