using Mango.Web.Models;
using Mango.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Mango.Web.Controllers
{
    public class CouponController : Controller
    {
        public ICouponService _coupService { get; set; }
        public CouponController(ICouponService coupservice)
        {
            _coupService = coupservice;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [ActionName("Details")]
        public async Task<IActionResult> CouponDetailsDefaultView()
        {
            List<CouponDTO>? couponsList = [];
            ResponseDTO response = await _coupService.GetAllCouponsAsync();
            TempData["ViewType"] = "Default";
            if (response.IsSuccess && response.Result != null)
            {
                couponsList = JsonConvert.DeserializeObject<List<CouponDTO>>(Convert.ToString(response.Result) ?? string.Empty);
                // TempData["SuccessMessage"] = "Coupons fetched successfully!";


                return View("Details", couponsList);
            }
            else
            {
                TempData["ErrorMessage"] = response.Message ?? "An error occurred while fetching the coupons.";
                return RedirectToAction(nameof(Error));
            }
        }
        [ActionName("CustomDetails")]
        public async Task<IActionResult> CouponDetailsCustomView()
        {
            List<CouponDTO>? couponsList = [];
            TempData["ViewType"] = "Custom";
            ResponseDTO response = await _coupService.GetAllCouponsAsync();
            //response.IsSuccess = false;
            if (response.IsSuccess && response.Result != null)
            {
                couponsList = JsonConvert.DeserializeObject<List<CouponDTO>>(Convert.ToString(response.Result) ?? string.Empty);

                //TempData["SuccessMessage"] = "Coupons fetched successfully!";

                return View("~/Views/Coupon/Custom/Details.cshtml", couponsList);
            }
            else
            {
                TempData["ErrorMessage"] = response.Message ?? "An error occurred while fetching the coupons.";
                return RedirectToAction(nameof(Error));
            }
        }

        [ActionName("Create")]
        public async Task<IActionResult> CreateCouponDefaultView()
        {
            return View("Create");
        }

        [ActionName("CustomCreate")]
        public async Task<IActionResult> CreateCouponCustomView()
        {

            return View("~/Views/Coupon/Custom/Create.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> CreateCouponDefaultView(CouponDTO coupon)
        {
            // ViewData["ImplementationType"] = "Default";
            TempData["ImplementationType"] = "Default";
            if (ModelState.IsValid)
            {
                ResponseDTO response = await _coupService.PostCouponDataAsync(coupon);

                if (response.IsSuccess && response.Result != null)
                {
                    var couponres = JsonConvert.DeserializeObject<CouponDTO>(Convert.ToString(response.Result));

                    TempData["Result"] =
                        JsonConvert.SerializeObject(couponres);


                    //TempData["Result"] = JsonConvert.SerializeObject(response.Result);
                    TempData["SuccessMessage"] = "Coupon created successfully!";
                    return RedirectToAction(nameof(Success));


                }
                else
                {
                    TempData["ErrorMessage"] = response.Message ?? "An error occurred while creating the coupon.";
                    return RedirectToAction("Create");
                }


            }

            return View("Create");
        }

        [HttpPost]
        public async Task<IActionResult> CreateCouponCustomView(CouponDTO coupon)
        {
            // ViewData["ImplementationType"] = "Custom";
            TempData["ImplementationType"] = "Custom";

            if (ModelState.IsValid)
            {
                ResponseDTO response = await _coupService.PostCouponDataAsync(coupon);


                if (response.IsSuccess && response.Result != null)
                {

                    var couponres = JsonConvert.DeserializeObject<CouponDTO>(Convert.ToString(response.Result));

                    TempData["Result"] =
                        JsonConvert.SerializeObject(couponres);

                    TempData["SuccessMessage"] = "Coupon created successfully!";
                    //TempData["Result"] = JsonConvert.SerializeObject(response.Result);

                    return RedirectToAction(nameof(Success));


                }
                else
                {
                    TempData["ErrorMessage"] = response.Message ?? "An error occurred while creating the coupon.";
                    return RedirectToAction("CustomCreate");
                }


            }

            return View("~/Views/Coupon/Custom/Create.cshtml");
        }


        public async Task<IActionResult> DeleteCoupon(int id)
        {
            ResponseDTO response = await _coupService.DeleteCouponAsync(id);
            if (response.IsSuccess)
            {
                TempData["SuccessMessage"] = "Coupon deleted successfully!";
                return RedirectToAction((TempData["ViewType"]?.ToString() == "Default") ? "Details" : "CustomDetails");
            }
            else
            {
                TempData["ErrorMessage"] = response.Message ?? "An error occurred while deleting the coupon.";
                return RedirectToAction(nameof(Error));
            }
        }

        public ActionResult Success()
        {
            var result = JsonConvert.DeserializeObject<CouponDTO>(TempData["Result"]?.ToString());
            return View(result);
        }


       
    }
}
