using AutoMapper;
using AutoMapper.QueryableExtensions;
using Mango.Services.CouponApi.Data;
using Mango.Services.CouponApi.Models;
using Mango.Services.CouponApi.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CouponAPIController : ControllerBase
    {
        public AppDBContext _dbcontxt { get; set; }
        public ResponseDTO _response { get; set; }
        public IMapper _map { get; set; }
        public CouponAPIController(AppDBContext dbcontxt,IMapper map)
        {
            _dbcontxt = dbcontxt;
            _response= new ResponseDTO();
            _map = map;
        }

        [Authorize]
        [HttpGet]
        public ResponseDTO GetCouponData()
        {
            try
            {
               
                IEnumerable<CouponDTO> objList = [.. _dbcontxt.Coupons.ProjectTo<CouponDTO>(_map.ConfigurationProvider)];
                // _response.Result = _map.Map<IEnumerable<CouponDTO>>(objList);
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

        public ResponseDTO GetCouponById(int id)
        {
            try
            {
               Coupon? obj= _dbcontxt.Coupons.Where(x => x.CouponId == id).FirstOrDefault();
                if(obj != null)
                 {
                      _response.Result = _map.Map<CouponDTO>(obj);
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

        [HttpGet("GetByCode/{code}")]
        public ResponseDTO GetCouponByCode(String code)
        {
            try
            {
                Coupon? obj = _dbcontxt.Coupons.Where(x => x.CouponCode.ToLower() == code.ToLower()).FirstOrDefault();
                if (obj != null)
                {
                    _response.Result = _map.Map<CouponDTO>(obj);
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

        [HttpPost("postcoupon")]

        public ResponseDTO CreateCoupon([FromBody] CouponDTO obj)
        {
            try
            {
                Coupon obj1 = _map.Map<Coupon>(obj);
                _dbcontxt.Coupons.Add(obj1);
                _dbcontxt.SaveChanges();
                _response.Result = _map.Map<CouponDTO>(obj1);
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message + "and Inner Exception: "+ ex.InnerException;
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpPut("updatecoupon")]

        public ResponseDTO UpdateCoupon([FromBody] CouponDTO obj)
        {
            try
            {
                Coupon obj1 = _map.Map<Coupon>(obj);
                _dbcontxt.Coupons.Update(obj1);
                _dbcontxt.SaveChanges();
                _response.Result = _map.Map<CouponDTO>(obj1);
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message + "and Inner Exception: " + ex.InnerException;
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpDelete("deletecoupon/{id:int}")]
        public ResponseDTO DeleteCoupon(int id)
        { 
            try
            {
                Coupon? obj1 = _dbcontxt.Coupons.Where(x => x.CouponId == id).FirstOrDefault();
                if (obj1 != null)
                {
                    _dbcontxt.Coupons.Remove(obj1);
                    _dbcontxt.SaveChanges();
                    _response.Result = _map.Map<CouponDTO>(obj1);
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
