using Mango.Web.Models;
using Mango.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Mango.Web.Controllers
{
    public class HomeController : Controller
    {
        public IProductService _productService { get; set; }
        public HomeController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductDTO>? productsList = [];

            ResponseDTO response = await _productService.GetAllProductsAsync();
            //response.IsSuccess = false;
            if (response.IsSuccess && response.Result != null)
            {
                productsList = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(response.Result) ?? string.Empty);

                //TempData["SuccessMessage"] = "Products fetched successfully!";

                return View(productsList);
            }
            else
            {
                TempData["ErrorMessage"] = response.Message ?? "An error occurred while fetching the products.";
                return RedirectToAction(nameof(Error));
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]


        public async Task<IActionResult> ProductDetails(int productId)
        {
            ProductDTO? product = null;

            ResponseDTO response = await _productService.GetProductByIdAsync(productId);
            //response.IsSuccess = false;
            if (response.IsSuccess && response.Result != null)
            {
                product = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result) ?? string.Empty);

                //TempData["SuccessMessage"] = "Products fetched successfully!";

                return View("MoreDetails", product);
            }
            else
            {
                TempData["ErrorMessage"] = response.Message ?? "An error occurred while fetching the products.";
                return RedirectToAction(nameof(Error));
            }
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
