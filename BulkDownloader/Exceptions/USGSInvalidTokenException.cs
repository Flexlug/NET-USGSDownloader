using System;
using System.Collections.Generic;
using System.Text;

namespace BulkDownloader.Exceptions
{
    /// <summary>
    /// Raised when authorization token is null or empty
    /// </summary>
    class USGSInvalidTokenException : Exception
    {
        public USGSInvalidTokenException() : base("Authorization token is null or empty") { }
        public USGSInvalidTokenException(string message) : base(message) { }
    }
}
