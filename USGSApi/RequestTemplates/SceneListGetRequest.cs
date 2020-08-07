using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace USGSApi.RequestTemplates
{
    public class SceneListGetRequest
    {
        [JsonProperty("listId")]
        public string ListId { get; set; }

        [JsonProperty("datasetName")]
        public string DatasetName { get; set; }
    }
}
