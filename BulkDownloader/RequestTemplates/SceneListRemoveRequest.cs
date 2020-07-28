using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace BulkDownloader.RequestTemplates
{
    public class SceneListRemoveRequest
    {
        [JsonProperty("listId")]
        public string ListId { get; set; }

        [JsonProperty("datasetName")]
        public string DatasetName { get; set; }

        [JsonProperty("entityId")]
        public string EntityId { get; set; }

        [JsonProperty("entityIds")]
        public List<string> EntityIds { get; set; }
    }
}
