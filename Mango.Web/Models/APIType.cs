namespace Mango.Web.Models
{
    public   class APIType
    {
        public static string? APIBaseUrl { get; set; }
        public enum APITypeEnum : int
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
