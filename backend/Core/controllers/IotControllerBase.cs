using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using AutoMapper;

using backend.Data;
using backend.Common;

namespace backend.Controllers
{
    /// <summary>
    /// Provides common ground for all service controllers.
    /// </summary>
    [ApiController]
    public class IotControllerBase : ControllerBase
    {
        /// <summary>
        /// Logger instance for debug tracing.
        /// </summary>       
        protected readonly ILogger<IotControllerBase> _logger;

        /// <summary>
        /// Configuration vault.
        /// </summary>
        protected readonly IConfiguration _configuration;

        /// <summary>
        /// Dto mapper service.
        /// </summary>
        protected readonly IMapper _mapper;

        /// <summary>
        /// Default class contructor. Receives internal services endpoints.
        /// </summary>
        /// <param name="configuration">Configuration instance from Startup. see AddSingleton help.</param>
        /// <param name="logger">Default logger instance.</param>
        /// <param name="mapper">Dto mapper resource.</param>        
        public IotControllerBase(IConfiguration configuration, ILogger<IotControllerBase> logger, IMapper mapper)
        {
            this._configuration = configuration;
            this._logger = logger;
            this._mapper = mapper;
        }

        /// <summary>
        /// Retrieves database connection string from configuration.
        /// </summary>
        /// <returns>A database connection string.</returns>
        private string GetDbConnectionString()
        {
            return _configuration.GetConnectionString("DBMySql");
        }

        /// <summary>
        /// Get the default application DB context.
        /// </summary>
        /// <returns>DB context instance.</returns>
        protected IotDbContext GetDbContext()
        {
            return new IotDbContext(GetDbConnectionString());
        }
    }
}

