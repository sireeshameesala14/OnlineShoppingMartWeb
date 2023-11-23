using Microsoft.AspNetCore.Mvc;
using OnlineShoppingMartWeb.Models;
using OnlineShoppingMartWeb.ResponseModels;
using OnlineShoppingMartWeb.Utility;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace OnlineShoppingMartWeb.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        const string UserSession = "UserSession";

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string apiResponse = ApiClient.GetAjaxResponse("User/GetUserAuthStatus?userName=" + login.UserName + "&password=" + login.Password + "&userType=user", "Get", string.Empty);
                    // string apiResponse = ApiClient.GetAjaxResponse("User/GetUserAuthStatus", "Get", JsonSerializer.Serialize(login));

                    if (!string.IsNullOrEmpty(apiResponse))
                    {
                        UserAuhDetail userAuthDetail = JsonSerializer.Deserialize<UserAuhDetail>(apiResponse);
                        userAuthDetail.UserName = login.UserName;
                        if (!userAuthDetail.IsAuthenticated)
                        {
                            return RedirectToAction("Login", "User", new { area = "" });
                        }
                        else
                        {
                            HttpContext.Session.SetObjectAsJson(UserSession, JsonSerializer.Serialize(userAuthDetail));
                        }
                    }
                }
                catch (Exception ex)
                {

                }


            }
            return View(login);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult SignOut()
        {
            HttpContext.Session.Remove("UserSession");
            HttpContext.Session.Remove("ProductDetail");
            HttpContext.Session.Remove("OrderDetail");
            HttpContext.Session.Remove("UserSession");
            return RedirectToAction("Index", "User", new { area = "" });
        }


        public IActionResult SignUp(RegistrationViewModel registrationModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    RegistrationInputModel input = new RegistrationInputModel();
                    input.UserName = registrationModel.Email;
                    input.Password = registrationModel.Password;
                    input.UserType = "user";
                    input.IsActive = true;
                    input.Email = registrationModel.Email;
                    input.FirstName = "";
                    input.LastName = "";
                    input.Address = "";
                    input.State = "";
                    input.City = "";
                    input.Country = "";
                    input.Mobile = "";
                    input.Pin = "";
                    string apiResponse = ApiClient.GetAjaxResponse("User/SaveUserRegistration", "POST", JsonSerializer.Serialize(input));
                    UserAuhDetail userAuthDetail = JsonSerializer.Deserialize<UserAuhDetail>(apiResponse);
                    if (!userAuthDetail.IsAuthenticated)
                    {
                        return RedirectToAction("Login", "User", new { area = "" });
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return View();
        }
        public IActionResult UserList()
        { 
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}