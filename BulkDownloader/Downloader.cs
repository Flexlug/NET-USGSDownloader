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
        private Token _token = null;

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

        /// <summary>
        /// This method is used to provide the contact information of the data owner.
        /// </summary>
        /// <param name="req">Structure with request params</param>
        /// <returns></returns>
        public DataOwnerResponse DataOwner(DataOwnerRequest req)
        {
            string content_json = JsonConvert.SerializeObject(req);
            RestRequest req_message = constructPostMessage("data-owner", content_json);
            IRestResponse message = sendRequest(req_message);

            DataOwnerResponse response = JsonConvert.DeserializeObject<DataOwnerResponse>(message.Content.ToString())
                                        ?? throw new USGSResponseNullException();

            return response;
        }

        public DatasetResponse Dataset(DatasetRequest req)
        {
            string content_json = JsonConvert.SerializeObject(req);
            RestRequest req_message = constructPostMessage("dataset", content_json);
            IRestResponse message = sendRequest(req_message);

            DatasetResponse response = JsonConvert.DeserializeObject<DatasetResponse>(message.Content.ToString())
                                        ?? throw new USGSResponseNullException();

            return response;
        }

        private void updateToken()
        {
            AuthRequest req = new AuthRequest(_email, _password);

            string content_json = JsonConvert.SerializeObject(req);
            RestRequest req_message = constructPostMessage("login", content_json, false);
            IRestResponse message = sendRequest(req_message);

            AuthResponse response = JsonConvert.DeserializeObject<AuthResponse>(message.Content.ToString()) 
                                        ?? throw new USGSResponseNullException();

            if (!string.IsNullOrEmpty(response.Token))
                _token = new Token(response.Token);
            else
                throw new USGSNoTokenException();
        }

        private IRestResponse sendRequest(RestRequest req_message)
        {
            IRestResponse message = _client.Execute(req_message);

            if (message.StatusCode == HttpStatusCode.OK)
                return message;
            else
                throw new USGSUnauthorizedException();
        }

        private RestRequest constructPostMessage(string url_end, string content, bool needToken = true)
        {
            RestRequest req_message = new RestRequest(USGS_URL + url_end, Method.POST);

            req_message.RequestFormat = DataFormat.Json;
            req_message.AddJsonBody(content);

            if (needToken)
            {
                if (!_token.IsValid())
                    updateToken();

                req_message.AddHeader("X-Auth-Token", _token.ToString());
            }

            return req_message;
        }
    }
}
