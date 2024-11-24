using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Keyfactor.Extensions.Orchestrator.FortiWeb.Models.Responses
{
    public class ErrorResponse
    {
        [JsonPropertyName("errcode")]
        public string ErrCode { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
