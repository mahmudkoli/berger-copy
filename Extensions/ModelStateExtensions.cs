using System.Collections.Generic;
using System.Linq;
using BergerMsfaApi.Core;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BergerMsfaApi.Extensions
{
    public static class ModelStateExtensions
    {
        public static List<ValidationError> GetErrors(this ModelStateDictionary modelState)
        {
            var errors = (from m in modelState
                          where m.Value.Errors.Count() > 0
                          select
                             new ValidationError
                             {
                                 PropertyName = m.Key,
                                 ErrorList = (from msg in m.Value.Errors
                                              select msg.ErrorMessage).ToArray()
                             })
                                .ToList();
            return errors;
        }
    }
}
