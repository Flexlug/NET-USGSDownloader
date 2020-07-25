using System;
using System.Net;
using System.Text;
using System.Net.Http;

using Newtonsoft.Json;

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
        /// Http client for communication purposes with USGS site
        /// </summary>
        private HttpClient _client = new HttpClient();

        /// <summary>
        /// URI to USGS API
        /// </summary>
        private readonly string USGS_URL = @"http://m2m.cr.usgs.gov/api/api/json/stable/";

        /// <summary>
        /// API Token, which is recieved while authorization
        /// </summary>
        private readonly string _token;

        /// <summary>
        /// Create Downloader instance and authorize on USGS site
        /// </summary>
        /// <param name="email">USGS confirmed account E-mail</param>
        /// <param name="password">USGS confirmed account password</param>
        public Downloader(string email, string password)
        {
            AuthRequest req = new AuthRequest(email, password);
            HttpRequestMessage req_message = new HttpRequestMessage(HttpMethod.Post, new Uri(USGS_URL + "login"));

            string content_json = JsonConvert.SerializeObject(req);
            req_message.Content = new StringContent(content_json, Encoding.Default, "application/json");

            HttpResponseMessage message = _client.SendAsync(req_message).Result;

            if (message.StatusCode == HttpStatusCode.OK)
            {
                AuthResponse response = JsonConvert.DeserializeObject<AuthResponse>(message.Content.ToString()) ?? throw new USGSResponseNullException(); ;

                if (!string.IsNullOrEmpty(response.Token))
                    _token = response.Token;
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
