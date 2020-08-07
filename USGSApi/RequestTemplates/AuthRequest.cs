using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace USGSApi.RequestTemplates
{
    /// <summary>
    /// JSON serializable request for authentification on USGS
    /// </summary>
    public class AuthRequest
    {
        [JsonProperty("username")]
        public string Login { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        public AuthRequest(string login, string password)
        {
            this.Login = login;
            this.Password = password;
        }
    }
}
