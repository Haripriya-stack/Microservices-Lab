namespace Mango.Web.Models
{
    public   class APIType
    {
        public static string? CouponAPIBaseUrl { get; set; }
        public static string? AuthAPIBaseUrl { get; set; }
        public enum APITypeEnum : int
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
