using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace BulkDownloader.ResponseTemplates
{
    public class SceneListGetResponse
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
            [JsonProperty("entityId")]
            public string EntityId { get; set; }

            [JsonProperty("datasetName")]
            public string DatasetName { get; set; }
        }
    }
}
