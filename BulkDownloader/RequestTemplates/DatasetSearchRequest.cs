using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace BulkDownloader.RequestTemplates
{
    /// <summary>
    /// Structure with dataset request params
    /// </summary>
    public class DatasetSearchRequest
    {
        [JsonProperty("catalog")]
        public string Catalog { get; set; }

        [JsonProperty("datasetName")]
        public string DatasetName { get; set; }

        [JsonProperty("includeMessages")]
        public bool? IncludeMessages { get; set; }

        [JsonProperty("publicOnly")]
        public bool? PublicOnly { get; set; }

        [JsonProperty("spatialFilter")]
        public SpatialFilterStruct SpatialFilter { get; set; }

        [JsonProperty("temporalFilter")]
        public TemporalFilterStruct TemporalFilter { get; set; }

        public class SpatialFilterStruct
        {
            [JsonProperty("filterType")]
            public string FilterType { get; set; }

            [JsonProperty("lowerLeft")]
            public Coordinates LowerLeft { get; set; }

            [JsonProperty("upperRight")]
            public Coordinates UpperRight { get; set; }

            public class Coordinates
            {
                [JsonProperty("latitude")]
                public double? Latitude { get; set; }

                [JsonProperty("longitude")]
                public double? Longitude { get; set; }
            }
        }

        public class TemporalFilterStruct
        {
            [JsonProperty("startDate")]
            public string StartDate { get; set; }

            [JsonProperty("endDate")]
            public string EndDate { get; set; }
        }
    }
}
