using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace USGSApi.RequestTemplates
{
    /// <summary>
    /// Structure with dataset request params
    /// </summary>
    public class DownloadOrderLoadRequest
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("downloadApplication")]
        public string DownloadApplication { get; set; }
    }
}
