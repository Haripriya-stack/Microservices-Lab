using Mango.Web.Models;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Web.Services
{
    public class BaseService : IBaseService
    {
        public IHttpClientFactory _httpClient { get; set; }

        public BaseService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ResponseDTO> SendAPIRequestAsync(RequestDTO request)
        {
            var client = _httpClient.CreateClient("CouponApi");

            HttpRequestMessage reqmessage = new HttpRequestMessage();

            reqmessage.Method = request.ApiTypeMethod switch
            {
                APIType.APITypeEnum.GET => HttpMethod.Get,
                APIType.APITypeEnum.POST => HttpMethod.Post,
                APIType.APITypeEnum.PUT => HttpMethod.Put,
                APIType.APITypeEnum.DELETE => HttpMethod.Delete,
                _ => throw new NotImplementedException(),
            };  
            reqmessage.Content= new StringContent(JsonConvert.SerializeObject(request.RequestBody??"") , Encoding.UTF8, request.ContentType);
            reqmessage.RequestUri = new Uri(request.Url??"");
            reqmessage.Headers.Add("Accept", request.ContentType);
           HttpResponseMessage result= await client.SendAsync(reqmessage);

            var apiResponse = new ResponseDTO
            {
                StatusCode = (int)result.StatusCode,
                IsSuccess = result.IsSuccessStatusCode,
                Message = result.ReasonPhrase
            };

            var responseContent = await result.Content.ReadFromJsonAsync<ResponseDTO>();


            return apiResponse;


        }
    }
}
