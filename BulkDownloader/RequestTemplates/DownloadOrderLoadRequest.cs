using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace BulkDownloader.RequestTemplates
{
    /// <summary>
    /// Structure with dataset request params
    /// </summary>
    public class DownloadOrderLoadRequest
    {
        [JsonProperty("label")]
        public List<string> Label { get; set; }

        [JsonProperty("downloadApplication")]
        public string DownloadApplication { get; set; }
    }
}
