using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace BulkDownloader.RequestTemplates
{
    /// <summary>
    /// Structure with dataset request params
    /// </summary>
    public class DatasetRequest
    {
        [JsonProperty("datasetId")]
        public string DatasetId { get; set; }

        [JsonProperty("datasetName")]
        public string DatasetName { get; set; }
    }
}
