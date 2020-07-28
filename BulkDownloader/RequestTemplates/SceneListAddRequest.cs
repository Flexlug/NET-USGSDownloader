using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulkDownloader.RequestTemplates
{
    public class SceneListAddRequest
    {
        [JsonProperty("listId")]
        public string ListId { get; set; }

        [JsonProperty("datasetName")]
        public string DatasetName { get; set; }

        [JsonProperty("idField")]
        public string IdField { get; set; }

        [JsonProperty("entityId")]
        public string EntityId { get; set; }

        [JsonProperty("entityIds")]
        public List<string> EntityIds { get; set; }
    }
}
