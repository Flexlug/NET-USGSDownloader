using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace BulkDownloader.ResponseTemplates
{
    /// <summary>
    /// Response for dataset-search request
    /// </summary>
    public class DatasetSearchResponse
    {
        [JsonProperty("errorCode")]
        public int? ErrorCode { get; set; }

        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }

        [JsonProperty("data")]
        public List<DataStruct> Data { get; set; }

        [JsonProperty("requestId")]
        public int? RequestId { get; set; }

        [JsonProperty("sessionId")]
        public int? SessionId { get; set; }

        public class DataStruct
        {
            [JsonProperty("catalogs")]
            public List<string> Catalogs { get; set; }

            [JsonProperty("keywords")]
            public string Keywords { get; set; }

            [JsonProperty("legacyId")]
            public string LegacyId { get; set; }

            [JsonProperty("dataOwner")]
            public string DataOwner { get; set; }

            [JsonProperty("datasetId")]
            public string DatasetId { get; set; }

            [JsonProperty("doiNumber")]
            public string DoiNumber { get; set; }

            [JsonProperty("sceneCount")]
            public int? SceneCount { get; set; }

            [JsonProperty("dateUpdated")]
            public string DateUpdated { get; set; }

            [JsonProperty("abstractText")]
            public string AbstractText { get; set; }

            [JsonProperty("datasetAlias")]
            public string DatasetAlias { get; set; }

            [JsonProperty("spatialBounds")]
            public SpatialBoundsStruct SpatialBounds { get; set; }

            [JsonProperty("acquisitionEnd")]
            public string AcquisitionEnd { get; set; }

            [JsonProperty("collectionName")]
            public string CollectionName { get; set; }

            [JsonProperty("ingestFrequency")]
            public string IngestFrequency { get; set; }

            [JsonProperty("acquisitionStart")]
            public string AcquisitionStart { get; set; }

            [JsonProperty("temporalCoverage")]
            public string TemporalCoverage { get; set; }

            [JsonProperty("supportCloudCover")]
            public bool? SupportCloudCover { get; set; }

            [JsonProperty("collectionLongName")]
            public string CollectionLongName { get; set; }

            [JsonProperty("datasetCategoryName")]
            public string DatasetCategoryName { get; set; }

            [JsonProperty("supportDeletionSearch")]
            public bool? SupportDeletionSearch { get; set; }

            public class SpatialBoundsStruct
            {
                [JsonProperty("east")]
                public double? East { get; set; }

                [JsonProperty("west")]
                public double? West { get; set; }

                [JsonProperty("north")]
                public double? North { get; set; }

                [JsonProperty("south")]
                public double? South { get; set; }
            }
        }
    }
}
