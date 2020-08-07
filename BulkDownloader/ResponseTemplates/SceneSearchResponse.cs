using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulkDownloader.ResponseTemplates
{
    public class SceneSearchResponse
    {
        public class DataStruct
        {
            [JsonProperty("results")]
            public List<ResultStruct> Results { get; set; }

            public class ResultStruct
            {
                [JsonProperty("browse")]
                public List<BrowseStruct> Browse { get; set; }

                [JsonProperty("entityId")]
                public string EntityId { get; set; }

                [JsonProperty("options")]
                public OptionsStruct Options { get; set; }

                public class OptionsStruct
                {
                    [JsonProperty("bulk")]
                    public bool? Bulk { get; set; }

                    [JsonProperty("download")]
                    public bool? Download { get; set; }

                    [JsonProperty("order")]
                    public bool? Order { get; set; }

                    [JsonProperty("secondary")]
                    public bool? Secondary { get; set; }
                }

                public class BrowseStruct
                {
                    [JsonProperty("id")]
                    public string Id { get; set; }

                    [JsonProperty("browseRotationEnabled")]
                    public bool? BrowseRotationsEnabled { get; set; }

                    [JsonProperty("browseName")]
                    public string BrowseName { get; set; }

                    [JsonProperty("browsePath")]
                    public string BrowsePath { get; set; }

                    [JsonProperty("overlayPath")]
                    public string OverlayPath { get; set; }

                    [JsonProperty("overlayType")]
                    public string OverlayType { get; set; }

                    [JsonProperty("thumbnailPath")]
                    public string ThumbnailPath { get; set; }
                }
            }

            [JsonProperty("totalHits")]
            public int? TotalHits { get; set; }

            [JsonProperty("nextRecord")]
            public int? NextRecord { get; set; }

            [JsonProperty("numExcluded")]
            public int? NumExcluded { get; set; }

            [JsonProperty("recordsReturned")]
            public int? RecordsReturned { get; set; }
        }

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
    }
}
 