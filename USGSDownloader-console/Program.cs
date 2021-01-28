using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Newtonsoft.Json;

using USGSApi;
using USGSApi.RequestTemplates;
using USGSApi.ResponseTemplates;

using USGSDownloader.InputTypes;

namespace USGSDownloader
{
    class Program
    {
        #region Automatic
        /*
        static void Main(string[] args)
        {
            Downloader downloader = null;
            ConsoleLogger logger = new ConsoleLogger();

            // Авторизация
            if (File.Exists("credentials.json"))
            {
                logger.LogInfo("main", "Starting authorization");

                LoginFile credentials;
                using (StreamReader sr = new StreamReader("credentials.json"))
                    credentials = JsonConvert.DeserializeObject<LoginFile>(sr.ReadToEnd()) ?? throw new Exception("Invalid credentails");

                downloader = new Downloader(credentials.Login, credentials.Password, logger);
                logger.LogSuccess("main", "Authorisation complete.");
            }

            // Считывание аргументов запуска программы
            if (args.Length != 0)
            {
                logger.LogInfo("main", "Got { args.Length} file paths.");
                foreach (string path in args)
                {
                    logger.LogInfo("main", $"Validating path {path}: ");
                    if (File.Exists(path))
                    {
                        logger.LogSuccess("main", $"Validated {path}. OK");

                        // Считываем входной json файл
                        string raw_json = string.Empty;
                        using (StreamReader sr = new StreamReader(path))
                            raw_json = sr.ReadToEnd();

                        logger.LogInfo("main", "Deserialization: ");
                        FeatureCollection fc = JsonConvert.DeserializeObject<FeatureCollection>(raw_json) ?? null;

                        if (fc == null)
                        {
                            logger.LogError("main", $"Deserialization failure. Check geojson file {path}");
                            logger.LogInfo("main", $"Skipping {path}");
                            continue;
                        }
                        else
                        {
                            logger.LogSuccess("main", $"Deserialized {path}. OK");
                        }

                        logger.LogInfo("main", "Requesting avaliable datasets for setted region: ");

                        // Ищем доступные dataset, из которых можно скачать снимки
                        DatasetSearchRequest dsr = new DatasetSearchRequest();
                        dsr.SpatialFilter = new DatasetSearchRequest.SpatialFilterStruct()
                        {
                            FilterType = "geojson",
                            GeoJson = new DatasetSearchRequest.SpatialFilterStruct.GeoJsonStruct()
                            {
                                Type = fc.Features[0].Geometry.Type,
                                Coordinates = fc.Features[0].Geometry.Coordinates
                            }
                        };
                        DatasetSearchResponse dsronse = downloader.DatasetSearch(dsr);

                        if (!string.IsNullOrEmpty(dsronse.ErrorMessage))
                        {
                            logger.LogError("main", $"Server return error: {dsronse.ErrorMessage}. Code: {dsronse.ErrorCode}");
                            logger.LogInfo("main", $"Skipping {path}");
                            continue;
                        }

                        if (dsronse.Data == null)
                        {
                            logger.LogError("main", $"No avaliable datasets");
                            logger.LogInfo("main", $"Skipping {path}");
                            continue;
                        }

                        logger.LogSuccess("main", $"Found {dsronse.Data.Count} databases");

                        List<string> categories = new List<string>();

                        int num = 1;
                        foreach(var dat in dsronse.Data)
                        {
                            if (!categories.Contains(dat.DatasetCategoryName))
                            {
                                Console.WriteLine($"{num++}:{dat.DatasetCategoryName}");
                                categories.Add(dat.DatasetCategoryName);
                            }
                        }

                        int chose = Convert.ToInt32(Console.ReadLine());

                        num = 1;
                        List<DatasetSearchResponse.DataStruct> dataList = new List<DatasetSearchResponse.DataStruct>();
                        foreach (var dat in dataList = dsronse.Data.Select(x => x)
                                                        .Where(x => x.DatasetCategoryName == categories[chose - 1])
                                                        .ToList())
                        {
                            Console.WriteLine($"{num++}: {dat.CollectionName}");
                        }

                        chose = Convert.ToInt32(Console.ReadLine());


                        // Берём первый датасет (обычно их очень много)
                        DatasetSearchResponse.DataStruct data = dataList[chose - 1];
                        logger.LogInfo("main", $"Using database. id: {data.DatasetId}, name: {data.DatasetAlias}");
                        logger.LogInfo("main", $"Requesting Entity Ids");

                        // Получаем ID картинок
                        List<SceneSearchResponse.DataStruct.ResultStruct> results = new List<SceneSearchResponse.DataStruct.ResultStruct>();
                        SceneSearchResponse sresp = downloader.SceneSearch(new SceneSearchRequest()
                        {
                            DatasetName = data.DatasetAlias,
                            SceneFilter = new SceneSearchRequest.SceneFilterStruct()
                            {
                                SpatialFilter = new SceneSearchRequest.SceneFilterStruct.SpatialFilterStruct()
                                {
                                    FilterType = "geojson",
                                    GeoJson = new SceneSearchRequest.SceneFilterStruct.SpatialFilterStruct.GeoJsonStruct()
                                    {
                                        Type = fc.Features[0].Geometry.Type,
                                        Coordinates = fc.Features[0].Geometry.Coordinates
                                    }
                                }
                            }
                        });

                        if (sresp.Data != null)
                        {
                            results.AddRange(sresp.Data.Results);
                        }
                        else
                        {
                            logger.LogError("main", "No results for current dataset");
                            logger.LogInfo("main", $"Skipping {path}");
                        }


                        logger.LogSuccess("main", $"Got {results.Count} entities");

                        num = 1;
                        foreach (var res in results)
                        {
                            Console.WriteLine($"{num}: {res.EntityId}");
                        }

                        Console.ReadLine();

                        // Качем каждую картинку по ID картинки и датасета
                        foreach (var res in results)
                        {
                            if (!res.Options.Download ?? true)
                                continue;

                            downloader.Download(data.DatasetId, res.EntityId);
                        }
                    }
                    else
                    {
                        logger.LogError("main", "Can not find file");
                        logger.LogInfo("main", $"Skipping {path}");
                    }
                }
            }
            else
            {
                logger.LogInfo("main", "No input files");
            }

            Console.WriteLine("Done");

            downloader.Logout();
        }
        */
        #endregion

        /**/
        #region Interactive

        static void Main(string[] args)
        {
            Downloader downloader = null;
            ConsoleLogger logger = new ConsoleLogger();

            // Авторизация
            if (File.Exists("credentials.json"))
            {
                logger.LogInfo("main", "Starting authorization");

                LoginFile credentials;
                using (StreamReader sr = new StreamReader("credentials.json"))
                    credentials = JsonConvert.DeserializeObject<LoginFile>(sr.ReadToEnd()) ?? throw new Exception("Invalid credentails");

                downloader = new Downloader(credentials.Login, credentials.Password, logger);
                logger.LogSuccess("main", "Authorisation complete.");
            }

            // Считывание аргументов запуска программы
            if (args.Length != 0)
            {
                logger.LogInfo("main", "Got { args.Length} file paths.");
                foreach (string path in args)
                {
                    logger.LogInfo("main", $"Validating path {path}: ");
                    if (File.Exists(path))
                    {
                        logger.LogSuccess("main", $"Validated {path}. OK");

                        // Считываем входной json файл
                        string raw_json = string.Empty;
                        using (StreamReader sr = new StreamReader(path))
                            raw_json = sr.ReadToEnd();

                        logger.LogInfo("main", "Deserialization: ");
                        FeatureCollection fc = JsonConvert.DeserializeObject<FeatureCollection>(raw_json) ?? null;

                        if (fc == null)
                        {
                            logger.LogError("main", $"Deserialization failure. Check geojson file {path}");
                            logger.LogInfo("main", $"Skipping {path}");
                            continue;
                        }
                        else
                        {
                            logger.LogSuccess("main", $"Deserialized {path}. OK");
                        }

                        logger.LogInfo("main", "Requesting avaliable datasets for setted region: ");

                        // Ищем доступные dataset, из которых можно скачать снимки
                        DatasetSearchRequest dsr = new DatasetSearchRequest();
                        dsr.SpatialFilter = new DatasetSearchRequest.SpatialFilterStruct()
                        {
                            FilterType = "geojson",
                            GeoJson = new DatasetSearchRequest.SpatialFilterStruct.GeoJsonStruct()
                            {
                                Type = fc.Features[0].Geometry.Type,
                                Coordinates = fc.Features[0].Geometry.Coordinates
                            }
                        };
                        DatasetSearchResponse dsronse = downloader.DatasetSearch(dsr);

                        if (!string.IsNullOrEmpty(dsronse.ErrorMessage))
                        {
                            logger.LogError("main", $"Server return error: {dsronse.ErrorMessage}. Code: {dsronse.ErrorCode}");
                            logger.LogInfo("main", $"Skipping {path}");
                            continue;
                        }

                        if (dsronse.Data == null)
                        {
                            logger.LogError("main", $"No avaliable datasets");
                            logger.LogInfo("main", $"Skipping {path}");
                            continue;
                        }

                        logger.LogSuccess("main", $"Found {dsronse.Data.Count} databases");

                        List<string> categories = new List<string>();

                        int num = 1;
                        foreach (var dat in dsronse.Data)
                        {
                            if (!categories.Contains(dat.DatasetCategoryName))
                            {
                                Console.WriteLine($"{num++}:{dat.DatasetCategoryName}");
                                categories.Add(dat.DatasetCategoryName);
                            }
                        }

                        int chose = Convert.ToInt32(Console.ReadLine());

                        num = 1;
                        List<DatasetSearchResponse.DataStruct> dataList = new List<DatasetSearchResponse.DataStruct>();
                        foreach (var dat in dataList = dsronse.Data.Select(x => x)
                                                        .Where(x => x.DatasetCategoryName == categories[chose - 1])
                                                        .ToList())
                        {
                            Console.WriteLine($"{num++}: {dat.CollectionName}");
                        }

                        chose = Convert.ToInt32(Console.ReadLine());


                        // Берём первый датасет (обычно их очень много)
                        DatasetSearchResponse.DataStruct data = dataList[chose - 1];
                        logger.LogInfo("main", $"Using database. id: {data.DatasetId}, name: {data.DatasetAlias}");
                        logger.LogInfo("main", $"Requesting Entity Ids");

                        // Получаем ID картинок
                        List<SceneSearchResponse.DataStruct.ResultStruct> results = new List<SceneSearchResponse.DataStruct.ResultStruct>();
                        SceneSearchResponse sresp = downloader.SceneSearch(new SceneSearchRequest()
                        {
                            DatasetName = data.DatasetAlias,
                            SceneFilter = new SceneSearchRequest.SceneFilterStruct()
                            {
                                SpatialFilter = new SceneSearchRequest.SceneFilterStruct.SpatialFilterStruct()
                                {
                                    FilterType = "geojson",
                                    GeoJson = new SceneSearchRequest.SceneFilterStruct.SpatialFilterStruct.GeoJsonStruct()
                                    {
                                        Type = fc.Features[0].Geometry.Type,
                                        Coordinates = fc.Features[0].Geometry.Coordinates
                                    }
                                }
                            }
                        });

                        if (sresp.Data != null)
                        {
                            results.AddRange(sresp.Data.Results);
                        }
                        else
                        {
                            logger.LogError("main", "No results for current dataset");
                            logger.LogInfo("main", $"Skipping {path}");
                        }


                        logger.LogSuccess("main", $"Got {results.Count} entities");

                        num = 1;
                        foreach (var res in results)
                        {
                            Console.WriteLine($"{num++}: {res.EntityId}");
                        }

                        Console.ReadLine();

                        // Качем каждую картинку по ID картинки и датасета
                        foreach (var res in results)
                        {
                            if (!res.Options.Download ?? true)
                                continue;

                            downloader.Download(data.DatasetId, res.EntityId);
                        }
                    }
                    else
                    {
                        logger.LogError("main", "Can not find file");
                        logger.LogInfo("main", $"Skipping {path}");
                    }
                }
            }
            else
            {
                logger.LogInfo("main", "No input files");
            }

            Console.WriteLine("Done");

            downloader.Logout();
        }

        #endregion
    }
}
