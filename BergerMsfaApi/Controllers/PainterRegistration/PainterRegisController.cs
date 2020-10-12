using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Services.PainterRegistration.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Controllers.PainterRegistration
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v1:apiVersion}/[controller]")]
    public class PainterRegisController:BaseController
    {
        private readonly IPainterRegistrationService _painterSvc;
        public PainterRegisController(
            IPainterRegistrationService painterSvc)
        {
            _painterSvc = painterSvc;
        }


        [HttpGet("GetPainterList")]
        public async Task<IActionResult> GetPainterList()
        {
            try
            {
                var result = await _painterSvc.GetPainterListAsync();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
