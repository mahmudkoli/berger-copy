using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BergerMsfaApi.Services.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.Common
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class CommonController : BaseController
    {
        private readonly ICommonService _commonSvc;
        public CommonController(
            ICommonService commonSvc)
        {
            _commonSvc = commonSvc;
        }

        [HttpGet("GetDealList")]
        public async Task<IActionResult> GetDealerList()
        {
            try
            {
                var result =await _commonSvc.GetDealerInfoList();
                return OkResult(result);
            }
            catch (Exception ex)
            {

                return ExceptionResult(ex);
            }
            
        }
    }
}
