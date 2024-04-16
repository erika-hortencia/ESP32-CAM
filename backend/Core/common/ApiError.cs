using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace backend.Common
{

    /// <summary>
    /// Internal error codes used to describe a given problem.
    /// </summary>
    public enum ApiErrorCode
    {
        // ------ Definir códigos de erro ------
        NO_ERROR = 200,
        INVALID_PARAMETERS = 1003,
        ERROR_PERSISTING_DATABASE = 2007,
    }

    /// <summary>
    /// A map of internal problems, description and final http response code.
    /// </summary>
    public class ApiError
    {
        /// <summary>
        /// Maps internal errors to http response codes.
        /// </summary>
        private readonly Dictionary<ApiErrorCode, Tuple<int, string>> _errorList = new Dictionary<ApiErrorCode, Tuple<int, string>>()
        {
            // ------ Definir códigos de erros/ mensagens de erro ------
            { ApiErrorCode.INVALID_PARAMETERS,  new Tuple<int, string>(400, "Invalid parameters")},
            { ApiErrorCode.ERROR_PERSISTING_DATABASE,  new Tuple<int, string>(422, "Error while trying to persist on database")}
        };

        /// <summary>
        /// Internal API error code.
        /// </summary>
        /// <value></value>
        public ApiErrorCode code { get; set; }

        /// <summary>
        /// Http status code.
        /// </summary>
        [JsonIgnore]
        public int status { get; set; }

        /// <summary>
        /// Message returned into response body.
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// Default class constructor.
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        public ApiError(ApiErrorCode errorCode, int statusCode, string message)
        {
            this.code = errorCode;
            this.status = statusCode;
            this.message = message;
        }

        /// <summary>
        /// Constructor from internal error code.
        /// </summary>
        /// <param name="errorCode"></param>
        public ApiError(ApiErrorCode errorCode)
        {
            code = errorCode;

            if (_errorList.ContainsKey(errorCode))
            {
                status = _errorList[errorCode].Item1;
                message = _errorList[errorCode].Item2;
            }
        }
    }
}