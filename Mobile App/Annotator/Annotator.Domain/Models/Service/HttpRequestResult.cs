using System.Net;

namespace Annotator.Domain.Models.Service
{
    public class HttpRequestResult<T>
    {
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
        public T Data { get; set; }
    }
}
