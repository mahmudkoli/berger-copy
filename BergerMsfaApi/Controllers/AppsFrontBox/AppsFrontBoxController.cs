using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Services.AppsFontBox;
using BergerMsfaApi.Services.Interfaces;

namespace BergerMsfaApi.Controllers.AppsFrontBox
{
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AppsFrontBoxController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly IAppFrontBoxService _appFrontBoxService;

        public AppsFrontBoxController(IAuthService authService,IAppFrontBoxService appFrontBoxService)
        {
            _authService = authService;
            _appFrontBoxService = appFrontBoxService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var area = _authService.GetLoggedInUserArea();
                var appFontBoxValue = await _appFrontBoxService.GetAppFontBoxValue(area);
                return OkResult(appFontBoxValue);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
