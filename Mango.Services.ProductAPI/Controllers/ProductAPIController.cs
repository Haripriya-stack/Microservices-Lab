using AutoMapper;
using AutoMapper.QueryableExtensions;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.DTO;
using Mango.Services.ProductAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Azure.Core.HttpHeader;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProductAPIController : ControllerBase
    {
        public AppDBContext _dbcontxt { get; set; }
        public ResponseDTO _response { get; set; }
        public IMapper _map { get; set; }
        public ProductAPIController(AppDBContext dbcontxt, IMapper map)
        {
            _dbcontxt = dbcontxt;
            _response = new ResponseDTO();
            _map = map;
        }

      //  [Authorize(Roles = "Admin,Customer")]
        [HttpGet]
        public ResponseDTO GetProductData()
        {
            try
            {

                IEnumerable<ProductDTO> objList = [.. _dbcontxt.Products.ProjectTo<ProductDTO>(_map.ConfigurationProvider)];
                // _response.Result = _map.Map<IEnumerable<ProductDTO>>(objList);
                _response.Result = objList;

            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;

            }
            return _response;

        }

        [HttpGet("GetById/{id:int}")]

        [Authorize(Roles = "Admin")]
        public ResponseDTO GetProductById(int id)
        {
            try
            {
                Product? obj = _dbcontxt.Products.Where(x => x.ProductId == id).FirstOrDefault();
                if (obj != null)
                {
                    _response.Result = _map.Map<ProductDTO>(obj);
                }
                else
                {
                    _response.Message = "Data Not Found";
                    _response.IsSuccess = false;
                }

            }
            catch (Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;

            }
            return _response;

        }

        

        [HttpPost("postproduct")]
     //   [Authorize(Policy = "AdminOnly")]
        public ResponseDTO CreateProduct([FromBody] ProductDTO obj)
        {
            try
            {
                Product obj1 = _map.Map<Product>(obj);
                _dbcontxt.Products.Add(obj1);
                _dbcontxt.SaveChanges();
                _response.Result = _map.Map<ProductDTO>(obj1);
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message + "and Inner Exception: " + ex.InnerException;
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpPut("updateproduct")]

        public ResponseDTO UpdateProduct([FromBody] ProductDTO obj)
        {
            try
            {
                Product obj1 = _map.Map<Product>(obj);
                _dbcontxt.Products.Update(obj1);
                _dbcontxt.SaveChanges();
                _response.Result = _map.Map<ProductDTO>(obj1);
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message + "and Inner Exception: " + ex.InnerException;
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpDelete("deleteproduct/{id:int}")]
        public ResponseDTO DeleteProduct(int id)
        {
            try
            {
                Product? obj1 = _dbcontxt.Products.Where(x => x.ProductId == id).FirstOrDefault();
                if (obj1 != null)
                {
                    _dbcontxt.Products.Remove(obj1);
                    _dbcontxt.SaveChanges();
                    _response.Result = _map.Map<ProductDTO>(obj1);
                }
                else
                {
                    _response.Message = "Data Not Found";
                    _response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message + "and Inner Exception: " + ex.InnerException;
                _response.IsSuccess = false;
            }
            return _response;
        }

    }
}
