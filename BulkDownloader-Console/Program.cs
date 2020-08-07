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
            Downloader downloader = new Downloader("flexlug", "Kjvjyjcjd123456789");

            if (args.Length != 0)
            {
                Console.WriteLine($"Got {args.Length} params.");
                foreach (string path in args)
                {
                    Console.Write($"Validating path {path}: ");
                    if (File.Exists(path))
                    {
                        Console.WriteLine("OK");

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

                        DatasetSearchResponse.DataStruct data = dsronse.Data.First();
                        Console.WriteLine($"Using database. id: {data.DatasetId}, name: {data.DatasetAlias}");

                        List<SceneSearchResponse.DataStruct.ResultStruct> results = new List<SceneSearchResponse.DataStruct.ResultStruct>();
                        do
                        {
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

                                if (sresp.Data.NextRecord == sresp.Data.TotalHits)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                Console.WriteLine("No results");
                                break;
                            }
                        } while (true);

                        Console.WriteLine($"Got {results.Count} entities");

                        foreach(var res in results)
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

            //downloader.Download(@"https://earthexplorer.usgs.gov/download/5e83a0ccb8e9e00e/TBDEMCI00218/EE/", "testfile1.zip");
            //Console.WriteLine("Press any key to start download");
            //Console.ReadKey();
            //downloader.Download("5e839f126ac633be", "SSR100991170071");
            //Console.ReadLine();
            //Console.WriteLine("OK");

            downloader.Logout();
        }
    }
}
