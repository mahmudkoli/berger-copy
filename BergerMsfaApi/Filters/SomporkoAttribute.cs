using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System;

namespace BergerMsfaApi.Filters
{
    public class SomporkoAttribute : Attribute
    {

        public SomporkoAttribute()
        {

        }
    }
}
