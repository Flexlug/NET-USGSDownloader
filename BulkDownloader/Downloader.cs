using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Net.Http;
using System.Text;

using Newtonsoft.Json;

using RestSharp;
using RestSharp.Extensions;

using BulkDownloader.Exceptions;
using BulkDownloader.RequestTemplates;
using BulkDownloader.ResponseTemplates;

using HtmlAgilityPack;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

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
        public readonly string DownloadApplication;

        /// <summary>
        /// Create Downloader instance and authorize on USGS site
        /// </summary>
        /// <param name="email">USGS confirmed account E-mail</param>
        /// <param name="password">USGS confirmed account password</param>
        public Downloader(string email, string password)
        {
            _email = email;
            _password = password;

            DownloadApplication = $"NET-BulkDownloader-{DateTime.Now.Year}-{DateTime.Now.DayOfWeek}";
            
            // Do not serialize null fields
            JsonSerializer.CreateDefault(new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            updateToken();
        }

        public DataOwnerResponse            DataOwner(DataOwnerRequest req)                     => MakeRequest<DataOwnerRequest, DataOwnerResponse>("data-owner", req) as DataOwnerResponse;
        public DatasetResponse              Dataset(DatasetRequest req)                         => MakeRequest<DatasetRequest, DatasetResponse>("dataset", req) as DatasetResponse;
        public DatasetCategoriesResponse    DatasetCategories(DatasetCategoriesRequest req)     => MakeRequest<DatasetCategoriesRequest, DatasetCategoriesResponse>("dataset-categories", req) as DatasetCategoriesResponse;
        public DatasetSearchResponse        DatasetSearch(DatasetSearchRequest req)             => MakeRequest<DatasetSearchRequest, DatasetSearchResponse>("dataset-search", req) as DatasetSearchResponse;
        public DownloadLabelsResponse       DownloadLabels(DownloadLabelsRequest req)           => MakeRequest<DownloadLabelsRequest, DownloadLabelsResponse>("download-labels", req) as DownloadLabelsResponse;
        public DownloadOrderLoadResponse    DownloadOrderLoad(DownloadOrderLoadRequest req)     => MakeRequest<DownloadOrderLoadRequest, DownloadOrderLoadResponse>("download-order-load", req) as DownloadOrderLoadResponse;
        public DownloadRetrieveResponse     DownloadRetrieve(DownloadRetrieveRequest req)       => MakeRequest<DownloadRetrieveRequest, DownloadRetrieveResponse>("download-retrieve", req) as DownloadRetrieveResponse;
        public SceneListAddResponse         SceneListAdd(SceneListAddRequest req)               => MakeRequest<SceneListAddRequest, SceneListAddResponse>("scene-list-add", req) as SceneListAddResponse;
        public SceneListGetResponse         SceneListGet(SceneListGetRequest req)               => MakeRequest<SceneListGetRequest, SceneListGetResponse>("scene-list-get", req) as SceneListGetResponse;
        public SceneListRemoveResponse      SceneListRemove(SceneListRemoveRequest req)         => MakeRequest<SceneListRemoveRequest, SceneListRemoveResponse>("scene-list-remove", req) as SceneListRemoveResponse;
        public SceneListSummaryResponse     SceneListSummary(SceneListSummaryRequest req)       => MakeRequest<SceneListSummaryRequest, SceneListSummaryResponse>("scene-list-summary", req) as SceneListSummaryResponse;
        public SceneSearchResponse          SceneSearch(SceneSearchRequest req)                 => MakeRequest<SceneSearchRequest, SceneSearchResponse>("scene-search", req) as SceneSearchResponse;

        /// <summary>
        /// Disactivate current token
        /// </summary>
        public void Logout()
        {
            // Send empty request with included token in header

            RestRequest req_message = new RestRequest(USGS_URL + "logout", Method.POST); 
            req_message.AddHeader("X-Auth-Token", _token.ToString());
            IRestResponse message = sendRequest(req_message);
        }

        //public void Download(string url, string file_path)
        public void Download(string datasetId, string entityId)
        {
            // Init client
            WebClient client = new WebClient();

            // Init first request
            RestRequest req1 = new RestRequest($@"https://earthexplorer.usgs.gov/scene/downloadoptions/{datasetId}/{entityId}/");
            req1.Method = Method.POST;

            Console.WriteLine($"Prepairing download - datasetId:{datasetId}, entityId:{entityId}");
            Console.WriteLine("Getting product id...");

            IRestResponse resp1 = _client.Execute(req1);

            // Parse
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(resp1.Content);
            HtmlNode node = doc.DocumentNode.SelectSingleNode(".//button[@class='btn btn-secondary downloadButton']");
            string productId = node.GetAttributeValue("data-productId", string.Empty);

            // Check if parsing successful
            if (string.IsNullOrEmpty(productId))
            {
                Console.WriteLine("Can't find product id");
                return;
            }

            Console.WriteLine($"Got product id. Value: {productId}");

            // Init seconds request
            //string req2_s = $@"https://earthexplorer.usgs.gov/download/{productId}/{entityId}/EE/";
            //Console.WriteLine($"Requesting json with download url from {req2_s}");
            //RestRequest req2 = new RestRequest(req2_s);
            //req2.RequestFormat = DataFormat.Json;
            //req2.Method = Method.POST;

            //IRestResponse resp2 = _client.Execute(req2);

            //DownloadMiddleState resp2_json = JsonConvert.DeserializeObject<DownloadMiddleState>(resp2.Content);

            //Console.WriteLine($"Got download url: {(string.IsNullOrEmpty(resp2_json.Url) ? string.Empty : resp2_json.Url)}");
            //if (string.IsNullOrEmpty(resp2_json.Url))
            //{
            //    Console.WriteLine("No url");
            //    return;
            //}

            string save_file = $"{entityId}-{DateTime.Now.Ticks}.ZIP";

            Console.WriteLine($"File will be saved as: {save_file}");
            Console.WriteLine("Starting download...");

            bool isDownloading = true;

            client.DownloadProgressChanged +=  (ev, e) =>
            {
                Task.Run(() =>
                {
                    DownloadProgressChangedEventArgs args = (DownloadProgressChangedEventArgs)e;
                    Console.WriteLine($"{String.Format("{0, 4}%  {1, 8}kb of {2, 8}kb", args.ProgressPercentage, args.BytesReceived / 1024, args.TotalBytesToReceive / 1024)}");
                });
            };
            client.DownloadFileCompleted += (ev, e) =>
            {
                Task.Run(() => 
                { 
                    Console.WriteLine("Download complete!");
                    isDownloading = false;
                });
            };
            //client.DownloadFileAsync(new Uri($@"{resp2_json.Url}dds_{((bool)resp2_json.IsPending ? "pending" : "download")}"), $"{entityId}.ZIP");

            //$"{entityId}.ZIP"

            client.DownloadFileAsync(new Uri($@"https://earthexplorer.usgs.gov/download/{productId}/{entityId}/EE/"), $"{save_file}");
            //client.DownloadData(req2).SaveAs("IK220050808060001M00.zip");

            // Stop thread while downloading
            while (isDownloading) { Thread.Sleep(1000); }
        }

        // WRONG APPLICATION ERROR (???)
        //
        //public string OrderSubmit()
        //{
        //    // Send empty request with included token in header

        //    RestRequest req_message = new RestRequest(USGS_URL + "order-submit", Method.POST);
        //    req_message.AddHeader("X-Auth-Token", _token.ToString());
        //    IRestResponse message = sendRequest(req_message);

        //    return message.Content;
        //}



        /// <summary>
        /// Make request to given endpoint url
        /// </summary>
        /// <typeparam name="T">Type of request structure</typeparam>
        /// <typeparam name="Y">Type of response structure</typeparam>
        /// <param name="end_url">Url end</param>
        /// <param name="req">Request structure</param>
        /// <returns></returns>
        private object MakeRequest<T, Y>(string end_url, T req)
        {
            string content_json = JsonConvert.SerializeObject(req);
            RestRequest req_message = constructPostMessage(end_url, content_json);
            IRestResponse message = sendRequest(req_message);

            Y response = JsonConvert.DeserializeObject<Y>(message.Content.ToString())
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
            {
                using (StreamWriter sr = new StreamWriter($"./responses/resp-{DateTime.Now.Ticks}.json"))
                    sr.WriteLine(message.Content);

                return message;
            }
            else 
            {
                throw new USGSUnauthorizedException(); 
            }
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
