using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BulkDownloader;
using BulkDownloader.RequestTemplates;
using BulkDownloader.ResponseTemplates;

using BulkDownloader_Console.InputTypes;
using Newtonsoft.Json;

namespace BulkDownloader_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Downloader downloader = null;

            // Авторизация
            if (File.Exists("credentials.json"))
            {
                LoginFile credentials;
                using (StreamReader sr = new StreamReader("credentials.json"))
                    credentials = JsonConvert.DeserializeObject<LoginFile>(sr.ReadToEnd()) ?? throw new Exception("Invalid credentails");

                downloader = new Downloader(credentials.Login, credentials.Password);
                Console.WriteLine("Login successful");
            }

            // Считывание аргументов запуска программы
            if (args.Length != 0)
            {
                Console.WriteLine($"Got {args.Length} params.");
                foreach (string path in args)
                {
                    Console.Write($"Validating path {path}: ");
                    if (File.Exists(path))
                    {
                        Console.WriteLine("OK");

                        // Считываем входной json файл
                        string raw_json = string.Empty;
                        using (StreamReader sr = new StreamReader(path))
                            raw_json = sr.ReadToEnd();

                        Console.Write("Deserialization: ");
                        FeatureCollection fc = JsonConvert.DeserializeObject<FeatureCollection>(raw_json) ?? null;

                        if (fc == null)
                        {
                            Console.WriteLine("FAIL");
                            Console.WriteLine($"Cannot deserialize geojson from file {path}");
                            continue;
                        }
                        else
                        {
                            Console.WriteLine("OK");
                        }

                        Console.Write("Requesting avaliable datasets for setted region: ");

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
                            Console.WriteLine("FAIL");
                            Console.WriteLine($"Server return error: {dsronse.ErrorMessage}. Code: {dsronse.ErrorCode}");
                            continue;
                        }

                        if (dsronse.Data == null)
                        {
                            Console.WriteLine("No avaliable databases");
                            continue;
                        }

                        Console.WriteLine($"Found {dsronse.Data.Count} databases");

                        // Берём первый датасет (обычно их очень много)
                        DatasetSearchResponse.DataStruct data = dsronse.Data.First();
                        Console.WriteLine($"Using database. id: {data.DatasetId}, name: {data.DatasetAlias}");

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
                            Console.WriteLine("No results");
                        }

                        Console.WriteLine($"Got {results.Count} entities");

                        // Качем каждую картинку по ID картинки и датасета
                        foreach (var res in results)
                        {
                            if (!res.Options.Download ?? true)
                                continue;

                            downloader.Download(data.DatasetId, res.EntityId);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("No input files");
            }

            Console.WriteLine("Complete!");

            downloader.Logout();
        }
    }
}
