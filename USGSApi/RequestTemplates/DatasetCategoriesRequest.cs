using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace USGSApi.RequestTemplates
{    
    /// <summary>
    /// Structure with dataset request params
    /// </summary>
    public class DatasetCategoriesRequest
    {
        /// <summary>
        /// Used to identify datasets that are associated with a given application
        /// </summary>
        [JsonProperty("catalog")]
        public string Catalog { get; set; }

        /// <summary>
        /// Optional parameter to include messages regarding specific dataset components
        /// </summary>
        [JsonProperty("includeMessages")]
        public bool? IncludeMessages { get; set; }

        /// <summary>
        /// Used as a filter out datasets that are not accessible to unauthenticated general public users
        /// </summary>
        [JsonProperty("publicOnly")]
        public bool? PublicOnly { get; set; }

        /// <summary>
        /// If provided, returned categories are limited to categories that are children of the provided ID
        /// </summary>
        [JsonProperty("parentId")]
        public string ParentId { get; set; }

        /// <summary>
        /// If provided, filters the datasets - this automatically adds a wildcard before and after the input value
        /// </summary>
        [JsonProperty("datasetFilter")]
        public string DatasetFilter { get; set; }
    }
}
