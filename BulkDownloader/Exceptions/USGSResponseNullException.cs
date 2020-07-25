using System;
using System.Collections.Generic;
using System.Text;

namespace BulkDownloader.Exceptions
{
    /// <summary>
    /// Raised when JSON auth response deserialisation returns null
    /// </summary>
    class USGSResponseNullException : Exception
    {
        public USGSResponseNullException() : base("JSON auth response deserialisation returned null") { }
        public USGSResponseNullException(string message) : base(message) { }
    }
}
