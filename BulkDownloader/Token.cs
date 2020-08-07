using System;
using System.Collections.Generic;
using System.Text;

namespace BulkDownloader
{
    /// <summary>
    /// Структура с токеном и временем его создания
    /// </summary>
    public class Token
    {
        private DateTime _creationTime;
        private string _token;

        /// <summary>
        /// Создать токен
        /// </summary>
        /// <param name="_token">Token</param>
        public Token(string _token)
        {
            _creationTime = DateTime.Now;
            this._token = _token;
        }

        /// <summary>
        /// Проверяет, можно ли ещё использовать токен (токен доступен только 2 часа)
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
