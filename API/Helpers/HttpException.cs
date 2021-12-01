using System;
using System.IO;

namespace API.Helpers
{
    public class HttpException : IOException
    {
        public HttpException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
        public HttpException(int statusCode, string message, Exception inner) : base(message, inner)
        {
            StatusCode = statusCode;
        }

        public int StatusCode { get; }
    }
}
