using System;
using System.Collections.Generic;
using System.Text;

namespace BulkDownloader.Exceptions
{
    /// <summary>
    /// Raised when authorization token is null or empty
    /// </summary>
    class USGSNoTokenException : Exception
    {
        public USGSNoTokenException() : base("Authorization token is null or empty") { }
        public USGSNoTokenException(string message) : base(message) { }
    }
}
