using System;
using System.Collections.Generic;
using System.Text;

namespace Blogger.MessagesModel
{
    public class ResponseResult<T> where T : class
    {
        public string Version { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public ErrorResult ResponseException { get; set; }
        public T Result { get; set; }
    }

    public class ErrorResult
    {
        public bool IsError { get; set; }
        public string ExceptionMessage { get; set; }
        public object DeveloperMeesage { get; set; }
        public string Details { get; set; }
        public string ReferenceErrorCode { get; set; }
        public IEnumerable<ValidationErrorResult> ValidationErrors { get; set; }
    }

    public class ValidationErrorResult
    {
        public string Field { get; }
        public string Message { get; }
        public ValidationErrorResult(string field, string message)
        {
            Field = field;
            Message = message;
        }


    }
}
