using System;
using System.Collections.Generic;
using System.Text;

namespace USGSApi.Intrefaces
{
    public interface IUSGSLogger
    {
        void LogInfo(string source, string message);
        void LogSuccess(string source, string message);
        void LogDownload(int percentage, long recievedBytes, long totalBytes);
        void LogError(string source, string message);
    }
}
