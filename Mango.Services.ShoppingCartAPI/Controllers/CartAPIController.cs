using AutoMapper;
using Mango.Services.ShoppingCartAPI.Data;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        public AppDBContext _dbcontxt { get; set; }
        public ResponseDTO _response { get; set; }
        public IMapper _map { get; set; }
        public CartAPIController(AppDBContext dbcontxt, IMapper map)
        {
            
            _dbcontxt = dbcontxt;
            _response = new ResponseDTO();
            _map = map;
        }

        public async Task<ResponseDTO> CartUpsert(CartDTO cartDTO)
        {
            try
            {
                //get any exisiting cardheader record for the given userid
                var cartHeaderFromDB = await _dbcontxt.CartHeaders.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == cartDTO.cartHeader.UserId);

                if(cartHeaderFromDB != null) {
                    // first get associated cartdetails by using cartheaderid and productid from the incoming cartDTO
                    //
                    var cartDetailsFromDB =await _dbcontxt.CartDetails.AsNoTracking().FirstOrDefaultAsync(c => c.CartHeaderId == cartHeaderFromDB.CartHeaderId 
                    && c.productId == cartDTO.cartDetailsList.First().productId);

                    if (cartDetailsFromDB != null)
                    {
                        //means cartdetails and cartheader both are present for the given userid and productid
                        //, so we need to update the count of the existing cartdetails record
                        cartDTO.cartDetailsList.First().count += cartDetailsFromDB.count;
                        cartDTO.cartDetailsList.First().CartDetailsId = cartDetailsFromDB.CartDetailsId;
                        cartDTO.cartDetailsList.First().CartHeaderId = cartDetailsFromDB.CartHeaderId;
                        _dbcontxt.CartDetails.Update(_map.Map<CartDetails>(cartDTO.cartDetailsList.First()));
                       await  _dbcontxt.SaveChangesAsync();
                    }
                    else
                    {
                        cartDTO.cartDetailsList.First().
                        CartHeaderId = cartDTO.cartHeader.CartHeaderId;
                        _dbcontxt.CartDetails.Add(_map.Map<CartDetails>(cartDTO.cartDetailsList.First()));
                        await _dbcontxt.SaveChangesAsync();
                    }
                }
                else {



                    // Create new cart header and card details
                    CartHeader newheader=_map.Map<CartHeader>(cartDTO.cartHeader);
                    _dbcontxt.CartHeaders.Add(newheader);
                    await _dbcontxt.SaveChangesAsync();

                    CartDetails newdetails = _map.Map<CartDetails>(cartDTO.cartDetailsList.First().
                        CartHeaderId=newheader.CartHeaderId);
                    _dbcontxt.CartDetails.Add(newdetails);
                    await _dbcontxt.SaveChangesAsync();
                }
                _response.IsSuccess = true;
                _response.Message = "Cart upserted successfully";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
    }
}
