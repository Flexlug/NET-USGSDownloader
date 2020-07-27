using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace BulkDownloader.ResponseTemplates
{
    /// <summary>
    /// Response for dataset-search request
    /// </summary>
    public class DownloadOrderLoadResponse
    {
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

        public class DataStruct
        {
            [JsonProperty("id")]
            public int? Id { get; set; }

            [JsonProperty("displayId`")]
            public string DisplayId { get; set; }

            [JsonProperty("entityId")]
            public string EntityId { get; set; }

            [JsonProperty("datasetId")]
            public string DatasetId { get; set; }

            [JsonProperty("available")]
            public string Available { get; set; }

            [JsonProperty("filesize")]
            public long? FileSize { get; set; }

            [JsonProperty("productName")]
            public string ProductName { get; set; }

            [JsonProperty("productCode")]
            public string ProductCode { get; set; }

            [JsonProperty("bulkAvailable")]
            public string BulkAvailable { get; set; }

            [JsonProperty("downloadSystem")]
            public string DownloadSystem { get; set; }

            [JsonProperty("secondaryDownloads")]
            public DataStruct secondaryDownloads { get; set; }
        }
    }
}
