using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NorbitsChallenge.Dal;
using NorbitsChallenge.Helpers;
using NorbitsChallenge.Models;

namespace NorbitsChallenge.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;

        public HomeController(IConfiguration config)
        {
            _config = config;
        }

        public IActionResult Index()
        {
            var model = GetCompanyModel();
            return View(model);
        }

        [HttpPost]
        public JsonResult FindCar(int companyId, string licensePlate)
        {
            /* var tireCount = new CarDb(_config).GetTireCount(companyId, licensePlate);

             var model = GetCompanyModel();
             model.TireCount = tireCount;

             return Json(model);*/

            var carInfo = new CarDb(_config).GetCarInfo(companyId, licensePlate);

            return Json(carInfo);
        }

        public JsonResult ShowAllCars(int companyId)
        {
            var allCars = new CarDb(_config).GetAllCars(companyId);

            return Json(allCars);
        }

        public JsonResult AddCar(int companyId, string licensePlate, string model, string brand, string description, int tireCount)
        {

            try
            {
                var isQuerySuccesfull = new CarDb(_config).AddCar(companyId, licensePlate, model, brand, description, tireCount);

            } catch (Exception ex)
            {
                return Json(ex);
            }
            var car = new CarModel { companyId = companyId, licensePlate = licensePlate, model = model, brand = brand, description = description, TireCount = tireCount };
            return Json(car);
        }

        public JsonResult EditCar(string licensePlate, string model, string brand, string description, int tireCount, string oldLicensePlate)
        {
            try
            {
                new CarDb(_config).EditCar(licensePlate, model, brand, description, tireCount, oldLicensePlate);

            } catch (Exception ex)
            {
                return Json(ex);
            }
            var car = new CarModel {licensePlate = licensePlate, model = model, brand = brand, description = description, TireCount = tireCount };
            return Json(car);
        }
        public JsonResult DeleteCar(string licensePlate)
        {

            try
            {
                 new CarDb(_config).DeleteCar(licensePlate);

            } catch (Exception ex)
            { 
                return Json(ex);
            }

            return Json(licensePlate);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

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

        private HomeModel GetCompanyModel()
        {
            var companyId = UserHelper.GetLoggedOnUserCompanyId();
            var companyName = new SettingsDb(_config).GetCompanyName(companyId);
            return new HomeModel { CompanyId = companyId, CompanyName = companyName };
        }
    }
}
