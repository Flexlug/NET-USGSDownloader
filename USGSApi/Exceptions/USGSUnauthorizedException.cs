using System;
using System.Net.Http;


namespace USGSApi.Exceptions
{
    /// <summary>
    /// Raised when authorizaton process did not return OK status code
    /// </summary>
    class USGSUnauthorizedException : Exception
    {
        public USGSUnauthorizedException() : base("Authorizaton process did not return OK status code") { }
        public USGSUnauthorizedException(string message) : base(message) { }
    }
}
