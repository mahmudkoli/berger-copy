using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.DemandGeneration;
using BergerMsfaApi.Services.DemandGeneration.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Controllers.DemandGeneration
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class LeadController : BaseController
    {
        private readonly ILeadService _leadService;

        public LeadController(
                ILeadService leadService
            )
        {
            this._leadService = leadService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] QueryObjectModel query)
        {
            try
            {
                var result = await _leadService.GetAllAsync(query);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
