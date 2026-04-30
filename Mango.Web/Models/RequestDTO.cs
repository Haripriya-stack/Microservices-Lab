using static Mango.Web.Models.APIType;

namespace Mango.Web.Models

{
    public class RequestDTO
    {
        public APITypeEnum ApiTypeMethod { get; set; } = APITypeEnum.GET;

       
        public string? Url { get; set; }

        public string ContentType { get; set; } = "application/json";

        public string? RequestBody { get; set; }
        public string? AccessToken { get; set; }
    }
}
