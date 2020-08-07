using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace USGSApi.RequestTemplates
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

        [JsonProperty("temporalFilter")]
        public TemporalFilterStruct TemporalFilter { get; set; }

        [JsonProperty("spatialFilter")]
        public SpatialFilterStruct SpatialFilter { get; set; }

        public class SpatialFilterStruct
        {
            [JsonProperty("filterType")]
            public string FilterType { get; set; }

            [JsonProperty("geoJson")]
            public GeoJsonStruct GeoJson { get; set; }

            public class GeoJsonStruct
            {
                [JsonProperty("type")]
                public string Type { get; set; }

                [JsonProperty("coordinates")]
                public List<List<List<double>>> Coordinates { get; set; }
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
