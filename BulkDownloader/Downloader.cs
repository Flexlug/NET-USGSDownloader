using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Net.Http;
using System.Text;

using Newtonsoft.Json;

using RestSharp;
using RestSharp.Extensions;

using USGSApi.Exceptions;
using USGSApi.RequestTemplates;
using USGSApi.ResponseTemplates;

using HtmlAgilityPack;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace USGSApi
{
    /// <summary>
    /// Главный класс для взаимодействия с USGS M2M API и загрузок
    /// </summary>
    public class Downloader
    {
        /// <summary>
        /// URL до USGS M2M API
        /// </summary>
        private const string USGS_URL = @"https://m2m.cr.usgs.gov/api/api/json/stable/";

        /// <summary>
        /// Http клиент для взаимодействия с API
        /// </summary>
        private IRestClient _client = new RestClient();

        /// <summary>
        /// API токен. Получается при авторизации
        /// </summary>
        private Token _token = null;

        /// <summary>
        /// E-Mail пользователя
        /// </summary>
        private readonly string _email;

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        private readonly string _password;

        /// <summary>
        /// Название приложения. Вкладывается в заголовок запроса
        /// </summary>
        public readonly string DownloadApplication;

        /// <summary>
        /// Создаёт экземпляр класса Downloader и получает токен
        /// </summary>
        /// <param name="email">E-mail от аккаунта USGS</param>
        /// <param name="password">Пароль от аккаунта USGS</param>
        public Downloader(string email, string password)
        {
            _email = email;
            _password = password;

            DownloadApplication = $"NET-USGSDownloader-{DateTime.Now.Year}-{DateTime.Now.DayOfWeek}";
            
            // Надстройка на JSON сериализатором. Игнорировать null поля
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
        /// Разлогиниться и аннулировать токен
        /// </summary>
        public void Logout()
        {
            // Отсылаем пустой запрос с токеном

            RestRequest req_message = new RestRequest(USGS_URL + "logout", Method.POST); 
            req_message.AddHeader("X-Auth-Token", _token.ToString());
            IRestResponse message = sendRequest(req_message);
        }

        /// <summary>
        /// Начать загрузку файла по ID снимка и ID датасета
        /// </summary>
        /// <param name="datasetId">ID датасета, в котором содержится снимок</param>
        /// <param name="entityId">ID снимка</param>
        public void Download(string datasetId, string entityId)
        {
            WebClient client = new WebClient();

            // Нет публичных методов, которые позволили бы качать снимки через USGS M2M API
            // Поэтому мы используем сервис USGS EarthExplorer, из которого мы вытягиваем ссылки на снимки
            // А для ссылок нам понадобятся раннее полученные id снимка и датасета

            // 1. Сперва получаем product id, потому что снимок может содержаться в нескольких форматах
            RestRequest req1 = new RestRequest($@"https://earthexplorer.usgs.gov/scene/downloadoptions/{datasetId}/{entityId}/");
            req1.Method = Method.POST;

            Console.WriteLine($"Prepairing download - datasetId:{datasetId}, entityId:{entityId}");
            Console.WriteLine("Getting product id...");

            IRestResponse resp1 = _client.Execute(req1);

            // TODO пусть качает все полученные product id а не самый первый попавшийся

            // Парсим product id
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(resp1.Content);
            HtmlNode node = doc.DocumentNode.SelectSingleNode(".//button[@class='btn btn-secondary downloadButton']");
            string productId = node.GetAttributeValue("data-productId", string.Empty);

            if (string.IsNullOrEmpty(productId))
            {
                Console.WriteLine("Can't find product id");
                return;
            }

            Console.WriteLine($"Got product id. Value: {productId}");
            
            // Качаем файл по product id

            string save_file = $"{entityId}-{DateTime.Now.Ticks}.ZIP";
            
            Console.WriteLine($"File will be saved as: {save_file}");
            Console.WriteLine("Starting download...");

            // Начинаем загрузку
            bool isDownloading = true;

            client.DownloadProgressChanged +=  (ev, e) =>
            {
                Task.Run(() =>
                {
                    DownloadProgressChangedEventArgs args = e;
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

            client.DownloadFileAsync(new Uri($@"https://earthexplorer.usgs.gov/download/{productId}/{entityId}/EE/"), $"{save_file}");

            // Тормозим поток, чтобы избежать размножения загрузок
            while (isDownloading) { Thread.Sleep(1000); }
        }


        /// <summary>
        /// Отправить запрос на выбранный URL endpoint
        /// </summary>
        /// <typeparam name="T">Тип JSON структуры запроса</typeparam>
        /// <typeparam name="Y">Тип JSON структуры ответа</typeparam>
        /// <param name="end_url">Url endpoint</param>
        /// <param name="req">Инстанс структуры с запросом</param>
        private object MakeRequest<T, Y>(string end_url, T req)
        {
            string content_json = JsonConvert.SerializeObject(req);
            RestRequest req_message = constructPostMessage(end_url, content_json);
            IRestResponse message = sendRequest(req_message);

            Y response = JsonConvert.DeserializeObject<Y>(message.Content.ToString())
                                        ?? throw new USGSResponseNullException();
            return response;
        }

        /// <summary>
        /// Запросить новый токен
        /// </summary>
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

        /// <summary>
        /// Отправить запрос на USGS M2M API
        /// </summary>
        /// <param name="req_message">Готовый http запрос</param>
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

        /// <summary>
        /// Собрать http запрос с нужными параметрами
        /// </summary>
        /// <param name="url_end">Url endpoint</param>
        /// <param name="content">JSON в виде строки</param>
        /// <param name="needToken">Если true, в загаловок запроса будет вложен токен</param>
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
