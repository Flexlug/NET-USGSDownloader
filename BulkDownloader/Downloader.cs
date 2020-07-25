using System;
using System.Net;
using System.Text;
using System.Net.Http;

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
        /// Http client for communication purposes with USGS site
        /// </summary>
        private IRestClient _client = new RestClient();

        /// <summary>
        /// URI to USGS API
        /// </summary>
        private readonly string USGS_URL = @"https://m2m.cr.usgs.gov/api/api/json/stable/";

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

            RestRequest req_message = new RestRequest(USGS_URL + "login", Method.POST);
            req_message.RequestFormat = DataFormat.Json;

            string content_json = JsonConvert.SerializeObject(req);
            req_message.AddJsonBody(content_json);

            //HttpRequestMessage req_message = new HttpRequestMessage(HttpMethod.Get, new Uri(USGS_URL + "login"));

            //req_message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("{ \"username\": \"flexlug\", \"password\": \"Kjvjyjcjd123456789\" }");


            IRestResponse message = _client.Execute(req_message);

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
