using System;

using BulkDownloader;

namespace BulkDownloader_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Downloader downloader = new Downloader("flexlug", "Kjvjyjcjd123456789");
            Console.WriteLine("OK");
        }
    }
}
