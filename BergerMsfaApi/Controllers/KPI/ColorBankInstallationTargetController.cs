using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Filters;
using BergerMsfaApi.Models.KPI;
using BergerMsfaApi.Models.Report;
using BergerMsfaApi.Services.KPI.interfaces;

namespace BergerMsfaApi.Controllers.KPI
{
    [AuthorizeFilter]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class ColorBankInstallationTargetController : BaseController
    {
        private readonly IColorBankInstallationTargetService _installationTargetService;

        public ColorBankInstallationTargetController(IColorBankInstallationTargetService installationTargetService)
        {
            _installationTargetService = installationTargetService;
        }


        [HttpGet("getTarget")]
        [ProducesResponseType(typeof(IList<ColorBankInstallationTargetSaveModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetTarget([FromQuery] ColorBankTargetSetupSearchModel model)
        {
            try
            {
                var result = await _installationTargetService.GetFyYearData(model);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }


        [HttpPost]
        [ProducesResponseType(typeof(IList<ColorBankProductivityBase>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> SaveOrUpdate(IList<ColorBankInstallationTargetSaveModel> model)
        {
            try
            {
                return OkResult(await _installationTargetService.SaveOrUpdate(model));
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
