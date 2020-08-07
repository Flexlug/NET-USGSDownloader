using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulkDownloader.ResponseTemplates
{
    /// <summary>
    /// Response for dataset-search request
    /// </summary>
    public class DownloadRetrieveResponse
    {
        [JsonProperty("data")]
        public DataStruct Data { get; set; }

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
            [JsonProperty("eulas")]
            public List<string> Eulas { get; set; }

            [JsonProperty("available")]
            public List<AvailableStruct> Available { get; set; }

            [JsonProperty("queueSize")]
            public int QueueSize { get; set; }

            [JsonProperty("requested")]
            public List<string> Requested { get; set; }

            public class AvailableStruct
            {
                [JsonProperty("url")]
                public string Url { get; set; }

                [JsonProperty("label")]
                public string Label { get; set; }

                [JsonProperty("entityId")]
                public string EntityId { get; set; }

                [JsonProperty("eulaCode")]
                public string EulaCode { get; set; }

                [JsonProperty("filesize")]
                public long? Filesize { get; set; }

                [JsonProperty("datasetId")]
                public string DatasetId { get; set; }

                [JsonProperty("displayId")]
                public string DisplayId { get; set; }

                [JsonProperty("downloadId")]
                public int DownloadId { get; set; }

                [JsonProperty("statusCode")]
                public string StatusCode { get; set; }

                [JsonProperty("statusText")]
                public string statusText { get; set; }

                [JsonProperty("productCode")]
                public string ProductCode { get; set; }

                [JsonProperty("productName")]
                public string ProductName { get; set; }

                [JsonProperty("collectionName")]
                public string CollectionName { get; set; }
            }
        }
    }
}
