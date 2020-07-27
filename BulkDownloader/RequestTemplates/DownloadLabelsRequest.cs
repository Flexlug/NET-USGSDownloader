using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulkDownloader.RequestTemplates
{
    /// <summary>
    /// Structure with dataset request params
    /// </summary>
    public class DownloadLabelsRequest
    {
        [JsonProperty("downloadApplication")]
        public string DownloadApplication { get; set; }
    }
}
