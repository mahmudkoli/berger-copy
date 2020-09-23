using System;

namespace Berger.Data.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public sealed class IgnoreUpdateAttribute : Attribute
    {
    }
}