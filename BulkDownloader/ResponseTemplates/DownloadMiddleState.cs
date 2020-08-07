using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace BulkDownloader.ResponseTemplates
{
    public class DownloadMiddleState
    {
        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }

        [JsonProperty("isPending")]
        public bool? IsPending { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
