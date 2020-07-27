using System;
using System.Net;
using System.Net.Http;
using System.Text;

using Newtonsoft.Json;

using RestSharp;

using BulkDownloader.Exceptions;
using BulkDownloader.RequestTemplates;
using BulkDownloader.ResponseTemplates;

namespace BulkDownloader
{
    /// <summary>
    /// Main class for download purposes
    /// </summary>
    public class Downloader
    {
        /// <summary>
        /// URI to USGS API
        /// </summary>
        private const string USGS_URL = @"https://m2m.cr.usgs.gov/api/api/json/stable/";

        /// <summary>
        /// Http client for communication purposes with USGS site
        /// </summary>
        private IRestClient _client = new RestClient();

        /// <summary>
        /// API Token, which is recieved while authorization
        /// </summary>
        private Token _token;

        /// <summary>
        /// User e-mail
        /// </summary>
        private readonly string _email;

        /// <summary>
        /// User password
        /// </summary>
        private readonly string _password;

        /// <summary>
        /// String representation of this application
        /// </summary>
        private readonly string downloadApplication;

        /// <summary>
        /// Create Downloader instance and authorize on USGS site
        /// </summary>
        /// <param name="email">USGS confirmed account E-mail</param>
        /// <param name="password">USGS confirmed account password</param>
        public Downloader(string email, string password)
        {
            _email = email;
            _password = password;

            downloadApplication = "NET-Core-BulkDownloader (C#) Platform/Windows";
            
            updateToken();
        }

        private void updateToken()
        {
            AuthRequest req = new AuthRequest(_email, _password);

            RestRequest req_message = new RestRequest(USGS_URL + "login", Method.POST);
            req_message.RequestFormat = DataFormat.Json;

            string content_json = JsonConvert.SerializeObject(req);
            req_message.AddJsonBody(content_json);

            IRestResponse message = _client.Execute(req_message);

            if (message.StatusCode == HttpStatusCode.OK)
            {
                AuthResponse response = JsonConvert.DeserializeObject<AuthResponse>(message.Content.ToString()) ?? throw new USGSResponseNullException(); ;

                if (!string.IsNullOrEmpty(response.Token))
                    _token = new Token(response.Token);
                else
                    throw new USGSInvalidTokenException();
            }
            else
            {
                throw new USGSUnauthorizedException();
            }
        }
    }
}
