using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ResponseMessageWrapper.Core.Wrappers
{
    [DataContract]
    public class ApiError
    {
        [DataMember]
        public bool IsError { get; set; }
        [DataMember]
        public string ExceptionMessage { get; set; }
        [DataMember]
        public object DeveloperMeesage { get; set; }
        [DataMember]
        public string Details { get; set; }
        [DataMember]
        public string ReferenceErrorCode { get; set; }
        [DataMember]
        public string ReferenceDocumentLink { get; set; }
        [DataMember]
        public IEnumerable<ValidationError> ValidationErrors { get; set; }
        public ApiError()
        {

        }
        public ApiError(string message)
        {
            this.ExceptionMessage = message;
            this.IsError = true;
        }

        public ApiError(ModelStateDictionary modelState)
        {
            this.IsError = true;
            if (modelState != null && modelState.Any(m => m.Value.Errors.Count > 0))
            {
                this.ExceptionMessage = "Please correct the specified validation errors and try again.";
                this.ValidationErrors = modelState.Keys
                .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
                .ToList();

            }
        }
    }
}
