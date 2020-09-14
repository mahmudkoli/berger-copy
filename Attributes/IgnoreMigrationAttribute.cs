using System;

namespace BergerMsfaApi.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class IgnoreMigrationAttribute : Attribute
    {
    }
}