using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace USGSApi.RequestTemplates
{
    /// <summary>
    /// Structure with data-owner request params
    /// </summary>
    public class DataOwnerRequest
    {
        /// <summary>
        /// Used to identify the data owner - this value comes from the dataset-search response
        /// </summary>
        [JsonProperty("dataOwner"), JsonRequired]
        public string DataOwner { get; set; }
    }
}
