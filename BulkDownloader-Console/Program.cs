using System;

using BulkDownloader;

namespace BulkDownloader_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Downloader downloader = new Downloader("flexlug", "Kjvjyjcjd123456789");
            BulkDownloader.ResponseTemplates.DataOwnerResponse resp = downloader.DataOwner(new BulkDownloader.RequestTemplates.DataOwnerRequest()
            {
                DataOwner = "DMID"
            });

            Console.WriteLine("OK");
        }
    }
}
