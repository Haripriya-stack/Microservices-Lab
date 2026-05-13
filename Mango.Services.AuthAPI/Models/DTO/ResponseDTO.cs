namespace Mango.Services.AuthAPI.Models.DTO
{
    public class ResponseDTO
    {
        public string Message { get; set; } = "";
        public object? Result { get; set; }
        public bool IsSuccess { get; set; } = true;
    }
}
