using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BergerMsfaApi.Filters
{
    public class GlobalActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //check workflow available
            var result = await next();

            //in

        }
    }
}
