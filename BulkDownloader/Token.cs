using System;
using System.Collections.Generic;
using System.Text;

namespace BulkDownloader
{
    /// <summary>
    /// Token storage with validation
    /// </summary>
    public class Token
    {
        private DateTime _creationTime;
        private string _token;

        /// <summary>
        /// Create token
        /// </summary>
        /// <param name="_token">Token</param>
        public Token(string _token)
        {
            _creationTime = DateTime.Now;
            this._token = _token;
        }

        /// <summary>
        /// Checks, if token is outdated
        /// </summary>
        public bool IsValid()
        {
            if (DateTime.Now - _creationTime < TimeSpan.FromHours(2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override string ToString() => _token;
    }
}
