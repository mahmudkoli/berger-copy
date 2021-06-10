using System;
using System.Collections.Generic;
using Berger.Common.Enumerations;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.Common
{
    [AuthorizeFilter]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class EnumController : BaseController
    {
        [HttpGet("GetEnumClubSupreme")]
        public IActionResult GetEnumClubSupreme()
        {
            try
            {
                List<EnumExtension.EnumProperty> res = EnumExtension.GetKeyValues(typeof(EnumClubSupreme));
                return OkResult(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
