namespace Mango.Web.Models
{
    public   class APIType
    {
        public static string? CouponAPIBaseUrl { get; set; }
        public static string? AuthAPIBaseUrl { get; set; } 

        public const string AdminRole="Admin";
        public const string CustomerRole = "Customer";
        public const string TokenKeyName = "AuthJWTToken";
        public enum APITypeEnum : int
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
