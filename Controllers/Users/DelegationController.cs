using System;
using System.Threading.Tasks;
using BergerMsfaApi.Controllers.Common;
using BergerMsfaApi.Filters;
using BergerMsfaApi.Models.Users;
using BergerMsfaApi.Services.Users.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BergerMsfaApi.Controllers.Users
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [JwtAuthorize]
    public class DelegationController : BaseController
    {
        private readonly ILogger<DelegationController> _logger;
        private readonly IDelegationService _delegation;

        public DelegationController(ILogger<DelegationController> logger, IDelegationService delegation)
        {
            _logger = logger;
            _delegation = delegation;
        }

        /// <summary>
        /// Return a list of Example Model objects
        /// </summary>
        /// <returns>ApiResponse</returns>
        [HttpGet("")]
        public async Task<IActionResult> GetDelegations()
        {
            try
            {
                var result = await _delegation.GetDelegationsAsync();
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        /// <summary>
        /// Return a list of Example Model objects
        /// </summary>
        /// <returns>ApiResponse</returns>
        [HttpGet("getnew")]
        public async Task<IActionResult> GetNewDelegations()
        {
            try
            {
                var result = await _delegation.GetNewDelegationsAsync();
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        /// <summary>
        /// Return a list of Example Model objects
        /// </summary>
        /// <returns>ApiResponse</returns>
        [HttpGet("getlogs")]
        public async Task<IActionResult> GetPastDelegations()
        {
            try
            {
                var result = await _delegation.GetPastDelegationsAsync();
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }




        /// <summary>
        /// return a single example object by exampleId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ApiResponse</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDelegation(int id)
        {
            try
            {

                var result = await _delegation.GetDelegationAsync(id);
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }

        }


        /// <summary>
        /// create or update Example object and Return a single of Example Model objects
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("save")]
        [ValidationFilter]
        public async Task<IActionResult> SaveDelegation([FromBody]DelegationModel model)
        {
            try
            {
                var isExist = await _delegation.IsCodeExistAsync(model.Id);
                if (isExist)
                {
                    ModelState.AddModelError(nameof(model.Id), "Already Exist");
                }
                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _delegation.SaveAsync(model);
                    return OkResult(result);
                }
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        /// <summary>
        /// update Delegation object and Return a single of Delegation Model objects
        /// </summary>
        /// <param name="model">DelegationModel</param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateDelegation([FromBody]DelegationModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationResult(ModelState);
                }
                else
                {
                    var result = await _delegation.UpdateAsync(model);
                    return OkResult(result);
                }
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }


        /// <summary>
        /// delete a single example object by exampleId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteDelegation(int id)
        {
            try
            {
                var result = await _delegation.DeleteAsync(id);
                return OkResult(result);

            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}