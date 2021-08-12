using Microsoft.AspNetCore.Mvc;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Filters;

namespace BergerMsfaApi.Controllers.KPI
{
    [AuthorizeFilter]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class ColorBankInstallationTargetController : BaseController
    {





    }
}
