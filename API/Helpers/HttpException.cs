using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace API.Helpers
{
    public class HttpException : IOException
    {
        public HttpException(string message, int statusCode = StatusCodes.Status400BadRequest) : base(message)
        {
            StatusCode = statusCode;
        }
        public HttpException(string message, Exception inner, int statusCode = StatusCodes.Status400BadRequest) : base(message, inner)
        {
            StatusCode = statusCode;
        }

        public int StatusCode { get; }
    }
}
