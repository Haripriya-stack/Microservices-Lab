using Mango.Web.Models;
using Mango.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        public IProductService _productService { get; set; }
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ProductDetails()
        {
            List<ProductDTO>? productsList = [];
           
            ResponseDTO response = await _productService.GetAllProductsAsync();
            //response.IsSuccess = false;
            if (response.IsSuccess && response.Result != null)
            {
                productsList = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(response.Result) ?? string.Empty);
                    
                //TempData["SuccessMessage"] = "Products fetched successfully!";

                return View("ProductDetails", productsList);
            }
            else
            {
                TempData["ErrorMessage"] = response.Message ?? "An error occurred while fetching the products.";
                return RedirectToAction(nameof(Error));
            }
        }

        public async Task<IActionResult> ProductCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDTO product)
        {
         

            if (ModelState.IsValid)
            {
                ResponseDTO response = await _productService.PostProductDataAsync(product);


                if (response.IsSuccess && response.Result != null)
                {

                    var productres = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));

                    TempData["Result"] =
                        JsonConvert.SerializeObject(productres);

                    TempData["SuccessMessage"] = "Product created successfully!";
                    //TempData["Result"] = JsonConvert.SerializeObject(response.Result);

                    return RedirectToAction(nameof(Success));


                }
                else
                {
                    TempData["ErrorMessage"] = response.Message ?? "An error occurred while creating the product.";
                    return View();
                }


            }

            return View();
        }


        public async Task<IActionResult> DeleteProduct(int id)
        {
            ResponseDTO response = await _productService.DeleteProductAsync(id);
            if (response.IsSuccess)
            {
                TempData["SuccessMessage"] = "Product deleted successfully!";
                return RedirectToAction( "Details" );
            }
            else
            {
                TempData["ErrorMessage"] = response.Message ?? "An error occurred while deleting the product.";
                return RedirectToAction(nameof(Error));
            }
        }

        public ActionResult Success()
        {
            var result = JsonConvert.DeserializeObject<ProductDTO>(TempData["Result"]?.ToString());
            return View(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
