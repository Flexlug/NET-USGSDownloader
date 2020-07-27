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
        /// <summary>
        /// The dataset identifier - must use this or datasetName
        /// </summary>
        [JsonProperty("datasetId")]
        public string DatasetId { get; set; }

        /// <summary>
        /// The system-friendly dataset name - must use this or datasetId
        /// </summary>
        [JsonProperty("datasetName")]
        public string DatasetName { get; set; }
    }
}
