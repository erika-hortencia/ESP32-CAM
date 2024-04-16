using System;
using System.Reflection;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using System.Net;
using System.Net.Mail;

using Swashbuckle.AspNetCore.Annotations;

using backend.Data;
using backend.Common;
using backend.Dto;
using backend.Service;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Collections.Generic;

namespace backend.Controllers
{
    /// <summary>
    /// The API authentication endpoint.
    /// </summary>
    [ApiController]
    [Route("Register")]
    public class RegisterController : IotControllerBase
    {
        /// <summary>
        /// Default class constructor
        /// </summary>
        /// <param name="logger">Logger instance</param>
        public RegisterController(IConfiguration configuration, ILogger<RegisterController> logger, IMapper mapper) :
            base(configuration, logger, mapper)
        {
        }

        /// <summary>
        /// Get all registers infos 
        /// </summary>
        /// <remarks>
        /// The method returns all infos about the registers
        /// </remarks>
        /// <response code="200">Get success</response>
        /// <response code="422">An exception on the backend database was detected</response>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<List<Register>> GetAllRegisters()
        {
            ActionResult actionResult = BadRequest(new ApiError(ApiErrorCode.INVALID_PARAMETERS));
            try
            {
                RegisterService registerService = new RegisterService(GetDbContext());

                List<Register> response = registerService.GetRegisters();

                actionResult = Ok(response);
            }
            catch (Exception ex)
            {
                Exception innerException = ex.GetOriginalException();
                if (innerException != null)
                {
                    ex = innerException;
                }

                actionResult =
                    UnprocessableEntity(
                        new ApiError(ApiErrorCode.ERROR_PERSISTING_DATABASE,
                        422,
                        ex.Message));

                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
            }

            return actionResult;
        }

        /// <summary>
        /// Create a new register
        /// </summary>
        /// <remarks>
        /// The method returns the infos of created register
        /// </remarks>
        /// <response code="200">Creation success</response>
        /// <response code="422">An exception on the backend database was detected</response>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult<List<Register>> CreateRegister([FromBody] RegisterInfosDto infos)
        {
            ActionResult actionResult = BadRequest(new ApiError(ApiErrorCode.INVALID_PARAMETERS));
            try
            {
                RegisterService registerService = new RegisterService(GetDbContext());

                Register response = registerService.CreateRegister(infos);

                actionResult = Ok(response);
            }
            catch (Exception ex)
            {
                Exception innerException = ex.GetOriginalException();
                if (innerException != null)
                {
                    ex = innerException;
                }

                actionResult =
                    UnprocessableEntity(
                        new ApiError(ApiErrorCode.ERROR_PERSISTING_DATABASE,
                        422,
                        ex.Message));

                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
            }

            return actionResult;
        }

        /// <summary>
        /// Update a register
        /// </summary>
        /// <remarks>
        /// The method returns the infos of updated register
        /// </remarks>
        /// <response code="200">Creation success</response>
        /// <response code="422">An exception on the backend database was detected</response>
        [HttpPut]
        [AllowAnonymous]
        public ActionResult<List<Register>> UpdateRegister([FromBody] RegisterUpdateDto infos)
        {
            ActionResult actionResult = BadRequest(new ApiError(ApiErrorCode.INVALID_PARAMETERS));
            try
            {
                RegisterService registerService = new RegisterService(GetDbContext());

                Register response = registerService.UpdateRegister(infos);

                actionResult = Ok(response);
            }
            catch (Exception ex)
            {
                Exception innerException = ex.GetOriginalException();
                if (innerException != null)
                {
                    ex = innerException;
                }

                actionResult =
                    UnprocessableEntity(
                        new ApiError(ApiErrorCode.ERROR_PERSISTING_DATABASE,
                        422,
                        ex.Message));

                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
            }

            return actionResult;
        }

        /// <summary>
        /// Delete a register
        /// </summary>
        /// <remarks>
        /// The method returns the infos of deleted register
        /// </remarks>
        /// <response code="200">Creation success</response>
        /// <response code="422">An exception on the backend database was detected</response>
        [HttpDelete]
        [AllowAnonymous]
        public ActionResult<List<Register>> DeleteRegister([FromBody] int id)
        {
            ActionResult actionResult = BadRequest(new ApiError(ApiErrorCode.INVALID_PARAMETERS));
            try
            {
                RegisterService registerService = new RegisterService(GetDbContext());

                Register response = registerService.DeleteRegister(id);

                actionResult = Ok(response);
            }
            catch (Exception ex)
            {
                Exception innerException = ex.GetOriginalException();
                if (innerException != null)
                {
                    ex = innerException;
                }

                actionResult =
                    UnprocessableEntity(
                        new ApiError(ApiErrorCode.ERROR_PERSISTING_DATABASE,
                        422,
                        ex.Message));

                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
            }

            return actionResult;
        }

    }
}
