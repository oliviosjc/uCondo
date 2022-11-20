using System.Net;

namespace uCondo.Application.DTOs
{
    public class ResponseViewModel
    {
        public ResponseViewModel()
        {

        }

        public dynamic Data { get; set; }

        public string Message { get; set; }

        public HttpStatusCode HttpStatusCode { get; set; }
    }
}
