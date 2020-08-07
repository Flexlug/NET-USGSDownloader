using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace BulkDownloader.ResponseTemplates
{
    /// <summary>
    /// Response for dataset request
    /// </summary>
    public class DownloadLabelsResponse
    {
        [JsonProperty("data")]
        public List<DataStruct> Data { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("errorCode")]
        public string ErrorCode { get; set; }

        [JsonProperty("requestId")]
        public int? RequestId { get; set; }

        [JsonProperty("sessionId")]
        public int? SessionId { get; set; }

        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }

        public class DataStruct
        {
            [JsonProperty("label")]
            public string Label { get; set; }

            [JsonProperty("dateEntered")]
            public int? DateEntered { get; set; }

            [JsonProperty("downloadSize")]
            public long? DownloadSize { get; set; }

            [JsonProperty("downloadCount")]
            public int? DownloadCount { get; set; }

            [JsonProperty("totalComplete")]
            public int? TotalComplete { get; set; }
        }
    }
}
