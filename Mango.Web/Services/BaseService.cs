using Mango.Web.Models;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Web.Services
{
    public class BaseService : IBaseService
    {
        public IHttpClientFactory _httpClient { get; set; }

        public ITokenStoreProvider _tokenStoreProvider { get; set; }

        public BaseService(IHttpClientFactory httpClient,ITokenStoreProvider tokenStoreProvider)
        {
            _httpClient = httpClient;
            _tokenStoreProvider=tokenStoreProvider;
        }
        public async Task<ResponseDTO> SendAPIRequestAsync(RequestDTO request,bool withBearer=true)
        {
            try
            {
                var client = _httpClient.CreateClient("CouponApi");

                HttpRequestMessage reqmessage = new HttpRequestMessage();

                reqmessage.Method = request.ApiTypeMethod switch
                {
                    APIType.APITypeEnum.GET => HttpMethod.Get,
                    APIType.APITypeEnum.POST => HttpMethod.Post,
                    APIType.APITypeEnum.PUT => HttpMethod.Put,
                    APIType.APITypeEnum.DELETE => HttpMethod.Delete,
                    _ => throw new NotImplementedException()
                };
                reqmessage.Content = new StringContent(JsonConvert.SerializeObject(request.RequestBody ?? ""), Encoding.UTF8, request.ContentType);
                reqmessage.RequestUri = new Uri(request.Url ?? "");
                reqmessage.Headers.Add("Accept", request.ContentType);
                if(withBearer)
                {
                   
                    string? JWTToken = _tokenStoreProvider.GetToken();
                    if (!string.IsNullOrEmpty(JWTToken))
                    {
                        reqmessage.Headers.Add("Authorization", $"Bearer {JWTToken}");
                    }
                };
      
                HttpResponseMessage result = await client.SendAsync(reqmessage);

                var apiResponse = new ResponseDTO
                {
                    StatusCode = (int)result.StatusCode,
                    IsSuccess = result.IsSuccessStatusCode,
                    Message = result.ReasonPhrase
                };
                if(result.Content.Headers.ContentLength == 0)
                {
                    return apiResponse;
                }
                else
                {
                    var serverResponse = await result.Content.ReadFromJsonAsync<ResponseDTO>();
                    apiResponse.Message = serverResponse?.Message ?? apiResponse.Message;
                    apiResponse.IsSuccess = serverResponse?.IsSuccess ?? apiResponse.IsSuccess;
                    apiResponse.Result = serverResponse?.Result;
                }
                


                // BaseService.SendAPIRequestAsync - replacement snippet
                //    var content = await result.Content.ReadAsStringAsync();

                // Deserialize the server response shape
                // var serverResponse = JsonConvert.DeserializeObject<ResponseDTO>(content);

                // Map serverResponse into the apiResponse we return to callers
               

                return apiResponse;
            }
            catch(Exception ex)
            {
                throw ex;
            }


        }
    }
}
