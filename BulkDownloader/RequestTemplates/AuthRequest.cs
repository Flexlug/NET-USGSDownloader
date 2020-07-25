using System;
using System.Collections.Generic;
using System.Text;

namespace BulkDownloader.RequestTemplates
{
    /// <summary>
    /// JSON serializable request for authentification on USGS
    /// </summary>
    public class AuthRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }

        public AuthRequest(string login, string password)
        {
            this.Login = login;
            this.Password = password;
        }
    }
}
