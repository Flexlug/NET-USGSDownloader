using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulkDownloader_Console.InputTypes
{
    public class LoginFile
    {
        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
