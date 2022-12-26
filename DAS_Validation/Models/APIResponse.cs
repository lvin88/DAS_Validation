using System.Net;

namespace DAS_Validation.Models
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public object Result { get; set; }
        public object ErrorMessage { get; set; }
    }
}
