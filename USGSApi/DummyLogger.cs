using System;
using System.Collections.Generic;
using System.Text;
using USGSApi.Intrefaces;

namespace USGSApi
{
    /// <summary>
    /// Пустая реализация логера, которая используется в случае, если в контрукторе класса Downloader не была указана ссылка на инстанс логера
    /// </summary>
    public class DummyLogger : IUSGSLogger
    {
        public void LogDownload(int percentage, long recievedBytes, long totalBytes) {  }
        public void LogError(string source, string message) { }
        public void LogInfo(string source, string message) { }
        public void LogSuccess(string source, string message) { }
    }
}
