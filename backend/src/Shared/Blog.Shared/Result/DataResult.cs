using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Shared.Result
{
    public class DataResult<T> : TResult<T>
    {
        public DataResult(T data, string message, int statusCode = 200)
        {
            Data = data;
            Message = message;
            StatusCode = statusCode;
            Success = true;
        }

        public bool Success { get; }
        public string Message { get; }
        public int StatusCode { get; }
        public T Data { get; }
    }
}