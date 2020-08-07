using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace BulkDownloader.ResponseTemplates
{
    public class SceneListSummaryResponse
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
            [JsonProperty("summary")]
            public SummaryStruct Summary { get; set; }

            [JsonProperty("datasets")]
            public List<DatasetStruct> Datasets { get; set; }

            public class DatasetStruct
            {
                [JsonProperty("sceneCount")]
                public string SceneCount { get; set; }

                [JsonProperty("datasetName")]
                public string DatasetName { get; set; }

                [JsonProperty("spatialBounds")]
                public SpatialBoundsStruct SpacialBounds { get; set; }

                [JsonProperty("temporalExtent")]
                public TemporalExtentStruct TemporalExtent { get; set; }
            }

            public class SummaryStruct
            {
                [JsonProperty("sceneCount")]
                public string SceneCount { get; set; }

                [JsonProperty("spatialBounds")]
                public SpatialBoundsStruct SpacialBounds { get; set; }

                [JsonProperty("temporalExtent")]
                public TemporalExtentStruct TemporalExtent { get; set; }
            }

            public class TemporalExtentStruct
            {
                [JsonProperty("max")]
                public string Max { get; set; }

                [JsonProperty("min")]
                public string Min { get; set; }
            }

            public class SpatialBoundsStruct
            {
                [JsonProperty("type")]
                public string Type { get; set; }

                [JsonProperty("coordinates")]
                public List<List<List<double?>>> Coordinates { get; set; }
            }
        }
    }
}
