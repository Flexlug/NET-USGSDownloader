using System;

using BulkDownloader;

namespace BulkDownloader_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Downloader downloader = new Downloader("flexlug", "Kjvjyjcjd123456789");

            //BulkDownloader.ResponseTemplates.DataOwnerResponse resp = downloader.DataOwner(new BulkDownloader.RequestTemplates.DataOwnerRequest()
            //{
            //    DataOwner = "DMID"
            //});

            //BulkDownloader.ResponseTemplates.DatasetResponse resp = downloader.Dataset(new BulkDownloader.RequestTemplates.DatasetRequest()
            //{
            //    DatasetName = "gls_all"
            //});

            //BulkDownloader.ResponseTemplates.DatasetSearchResponse resp;
            //resp = downloader.DatasetSearch(new BulkDownloader.RequestTemplates.DatasetSearchRequest()
            //{
            //    DatasetName = "Global",
            //    SpatialFilter = new BulkDownloader.RequestTemplates.DatasetSearchRequest.SpatialFilterStruct()
            //    {
            //        FilterType = "mbr",
            //        LowerLeft = new BulkDownloader.RequestTemplates.DatasetSearchRequest.SpatialFilterStruct.Coordinates()
            //        {
            //            Latitude = 44.60847,
            //            Longitude = -99.69639
            //        },
            //        UpperRight = new BulkDownloader.RequestTemplates.DatasetSearchRequest.SpatialFilterStruct.Coordinates()
            //        {
            //            Latitude = 44.60847,
            //            Longitude = -99.69639
            //        }
            //    }
            //});

            downloader.DownloadRetrieve(new BulkDownloader.RequestTemplates.DownloadRetrieveRequest()
            {
                DownloadApplication = downloader.DownloadApplication
            });

            downloader.Logout();

            Console.WriteLine("OK");
        }
    }
}
