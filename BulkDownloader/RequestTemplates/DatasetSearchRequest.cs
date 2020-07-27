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


    }
}
