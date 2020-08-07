using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace USGSApi.ResponseTemplates
{
    /// <summary>
    /// Response for dataset-search request
    /// </summary>
    public class DownloadOrderLoadResponse
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

            [JsonProperty("entityId")]
            public string EntityId { get; set; }

            [JsonProperty("eulaCode")]
            public string Eialcode { get; set; }

            [JsonProperty("filesize")]
            public long? FileSize { get; set; }

            [JsonProperty("datasetId")]
            public string DatasetId { get; set; }

            [JsonProperty("displayId`")]
            public string DisplayId { get; set; }

            [JsonProperty("statusCode")]
            public string StatusCode { get; set; }

            [JsonProperty("statusText")]
            public string StatusText { get; set; }

            [JsonProperty("productCode")]
            public string ProductCode { get; set; }

            [JsonProperty("productName")]
            public string ProductName { get; set; }

            [JsonProperty("collectionName")]
            public string CollectionName { get; set; }
        }
    }
}
