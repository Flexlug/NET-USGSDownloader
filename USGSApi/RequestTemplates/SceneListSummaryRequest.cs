using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace USGSApi.RequestTemplates
{
    public class SceneListSummaryRequest
    {
        [JsonProperty("listId"), JsonRequired]
        public string ListId { get; set; }

        [JsonProperty("datasetName")]
        public string DatasetName { get; set; }
    }
}
