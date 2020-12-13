using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Models.Tintining;
using BergerMsfaApi.Services.Tintining.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BergerMsfaApi.Controllers.Tintining
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class AppTintingMachineController : BaseController
    {
        private readonly ILogger<AppTintingMachineController> _logger;
        private readonly ITintiningService _tintiningService;
        public AppTintingMachineController
            (
            ILogger<AppTintingMachineController> logger,
            ITintiningService tintiningService
            )
        {
            _tintiningService = tintiningService;
            _logger = logger;
        }
      
        [HttpGet("GetTininingMachineList/{territory}")]
        public async Task< IActionResult> GetTininingMachineList([BindRequired]string territory)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                 var result = await _tintiningService.AppGetTintingMachineList(territory);
                return OkResult(result);
            }
            catch (System.Exception ex)
            {

                return ExceptionResult(ex);
            }
        }

        [HttpGet("CreateTiningMachine")]
        public async Task<IActionResult> CreateTiningMachine(string  employeeId, string territory="T000")
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                var result = await _tintiningService.AppCreateTiningMachine(employeeId, territory);
                return OkResult(result);
            }
            catch (System.Exception ex)
            {

                return ExceptionResult(ex); 
            }
        }

        [HttpPost("CreateTiningMachine")]
        public async Task<IActionResult> CreateTiningMachine([FromBody] List<TintiningMachineModel> model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);
                var result = await _tintiningService.AppCreateTiningMachine(model);
                return OkResult(result);
            }
            catch (System.Exception ex)
            {

                return ExceptionResult(ex);
            }
        }

        [HttpPut("UpdateTitningMachine")]
        public async Task<IActionResult> UpdateTitningMachine([FromBody] List< TintiningMachineModel> model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);

                //if (! await _tintiningService.IsTitiningMachineUpdatable(model))
                //{
                //    ModelState.AddModelError(nameof(model.CompanyId), "you have already excced update limt");
                //    return ValidationResult(ModelState);
                //}
                var result = await _tintiningService.AppUpdateTitningMachine(model);
                return OkResult(result);
            }
            catch (System.Exception ex)
            {

                return ExceptionResult(ex);
            }
        }

        [HttpPut("IsTitiningMachineUpdatable")]
        public async Task<IActionResult> IsTitiningMachineUpdatable([FromBody] TintiningMachineModel model)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);

             var result=await _tintiningService.IsTitiningMachineUpdatable(model);
              
                return OkResult(result);
            }
            catch (System.Exception ex)
            {

                return ExceptionResult(ex);
            }
        }
        [HttpDelete("DeleteTitningMachine/{Id}")]
        public async Task<IActionResult> DeleteTitningMachine([BindRequired]int Id )
        {
            try
            {
                if (!ModelState.IsValid) return ValidationResult(ModelState);

                if (await _tintiningService.IsExits(Id))
                {
                    ModelState.AddModelError(nameof(Id), "does not exist");
                    return ValidationResult(ModelState);
                }
                var result = await _tintiningService.DeleteTitiningMachine(Id);
                return OkResult(result);
            }
            catch (System.Exception ex)
            {

                return ExceptionResult(ex);
            }
        }
    }
}
