using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace USGSDownloader.InputTypes
{
    public class FeatureCollection
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("crs")]
        public ItemStruct Crs { get; set; }

        [JsonProperty("features")]
        public List<ItemStruct> Features { get; set; }

        public class ItemStruct
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("properties")]
            public PropertiesStruct Properties { get; set; }

            [JsonProperty("geometry")]
            public GeometryStruct Geometry { get; set; }

            public class GeometryStruct
            {
                [JsonProperty("type")]
                public string Type { get; set; }

                [JsonProperty("coordinates")]
                public List<List<List<double>>> Coordinates { get; set; }
            }

            public class PropertiesStruct
            {
                [JsonProperty("name")]
                public string Name { get; set; }

                [JsonProperty("id")]
                public int? Id { get; set; }
            }
        }
    }
}
