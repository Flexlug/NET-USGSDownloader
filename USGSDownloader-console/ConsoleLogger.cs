using System;
using System.Collections.Generic;
using System.Text;

using USGSApi.Intrefaces;

namespace USGSDownloader
{
    public class ConsoleLogger : IUSGSLogger
    {
        // Необходим для потокобезопасности
        private object threadLock = new object();

        public void LogError(string source, string message)
        {
            lock (threadLock)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(String.Format("{0, 9} ", DateTime.Now.ToLongTimeString()));
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(String.Format("{0, 11}", "[ERROR] "));
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($" [{source}] {message}");
                Console.WriteLine();
            }
        }

        public void LogSuccess(string source, string message)
        {
            lock (threadLock)
            {
                Console.Write(String.Format("{0, 9} ", DateTime.Now.ToLongTimeString()));
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(String.Format("{0, 11}", "[OK] "));
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($" [{source}] {message}");
                Console.WriteLine();
            }
        }

        public void LogInfo(string source, string message)
        {
            lock (threadLock)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(String.Format("{0, 9} ", DateTime.Now.ToLongTimeString()));
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(String.Format("{0, 11}", "[INFO] "));
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($" [{source}] {message}");
                Console.WriteLine();
            }
        }

        public void LogDownload(int percentage, long recievedBytes, long totalBytes)
        {
            lock (threadLock)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(String.Format("{0, 9} ", DateTime.Now.ToLongTimeString()));
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(String.Format("{0, 11}", "[DOWNLOAD] "));
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($" {percentage}% {recievedBytes / 1024} kb // {totalBytes / 1024} kb");
                Console.WriteLine();
            }
        }
    }
}
