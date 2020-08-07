using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace USGSApi.RequestTemplates
{
    public class SceneSearchRequest
    {
        [JsonProperty("datasetName"), JsonRequired]
        public string DatasetName { get; set; }

        [JsonProperty("maxResults")]
        public int? MaxResults { get; set; }

        [JsonProperty("startingNumber")]
        public int? StartingNumber { get; set; }

        [JsonProperty("metadataType")]
        public string MetadataType { get; set; }

        [JsonProperty("sortField")]
        public string SortField { get; set; }

        [JsonProperty("sortDirection")]
        public string SortDirection { get; set; }

        [JsonProperty("sceneFilter")]
        public SceneFilterStruct SceneFilter { get; set; }

        public class SceneFilterStruct
        {
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
        }

        [JsonProperty("compareListName")]
        public string CompareListName { get; set; }

        [JsonProperty("bulkListName")]
        public string BulkListName { get; set; }

        [JsonProperty("orderListName")]
        public string OrderListName { get; set; }

        [JsonProperty("excludeListName")]
        public string ExcludeListName { get; set; }
    }
}
