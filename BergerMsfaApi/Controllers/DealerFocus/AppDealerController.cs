using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.Common.Interfaces;
using BergerMsfaApi.Services.DealerFocus.Implementation;
using Microsoft.AspNetCore.Mvc;

namespace BergerMsfaApi.Controllers.DealerFocus
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AppDealerController : BaseController
    {
        private readonly ICommonService _commonSvc;

        public AppDealerController(ICommonService commonSvc)
        {
            _commonSvc = commonSvc;
        }

        //this method expose dealer list by territory for App
        [HttpGet("GetDealList/{territory}")]
        public async Task<IActionResult> GetDealerList(string territory)
        {
            try
            { 
                if(string.IsNullOrEmpty(territory))
                {
                    ModelState.AddModelError(nameof(territory), "territory can not be null");
                    return ValidationResult(ModelState);
                }
                var result = await _commonSvc.GeApptDealerInfoList(territory.Trim());
                return OkResult(result);
            }
            catch (Exception ex)
            {

                return ExceptionResult(ex);
            }

        }

    }
}
