using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoHub.Shared.Models
{
    public class Response : Response<object> { }

    public class Response<T>
    {
        public T? Data { get; set; }
        public bool IsSuccess => !Errors.Any();
        public List<ResponseError> Errors { get; set; } = new List<ResponseError>();
        public void AddError(string message) => Errors.Add(new ResponseError { Code = ResponseErrorCode.Failed, Message = message, });
        public void AddError(Exception ex) => Errors.Add(new ResponseError { Code = ResponseErrorCode.Failed, Message = ex.InnerException?.Message ?? ex.Message, Exception = ex, });
        public void AddError(ResponseErrorCode errorCode) => Errors.Add(new ResponseError { Code = errorCode, Message = errorCode.ToString(), });
        public string GetErrors => string.Join(", ", Errors.Select(x => x.Message));
    }

    public class ResponseError
    {
        public ResponseErrorCode Code { get; set; }
        public string? Message { get; set; }
        public Exception? Exception { get; set; }
    }

    public enum ResponseErrorCode
    {
        Failed = -1,
        NotFound = -2,
    }
}
