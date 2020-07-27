using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace BulkDownloader.ResponseTemplates
{
    /// <summary>
    /// Response for data-owner request
    /// </summary>
    public class DataOwnerResponse
    {
        public class DataStruct
        {
            [JsonProperty("city")]
            public string City { get; set; }

            [JsonProperty("email")]
            public string Email { get; set; }

            [JsonProperty("phone")]
            public string Phone { get; set; }

            [JsonProperty("address")]
            public string Address { get; set; }

            [JsonProperty("country")]
            public string Country { get; set; }

            [JsonProperty("postalCode")]
            public string PostalCode { get; set; }

            [JsonProperty("contactName")]
            public string ContactName { get; set; }

            [JsonProperty("organizationName")]
            public string OrganizationName { get; set; }
        }

        [JsonProperty("data")]
        public DataStruct Data { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("errorCode")]
        public int? ErrorCode { get; set; }

        [JsonProperty("requestId")]
        public int? RequestId { get; set; }

        [JsonProperty("sessionId")]
        public int? SessionId { get; set; }

        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }
    }
}
