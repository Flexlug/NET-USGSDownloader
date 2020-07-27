using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace BulkDownloader.RequestTemplates
{    
    /// <summary>
    /// Structure with dataset request params
    /// </summary>
    public class DownloadRetrieveRequest
    {
        [JsonProperty("downloadApplication")]
        public string DownloadApplication { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }
    }
}
