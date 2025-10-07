using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using System.Net.Http;

namespace ECommerceFrontend.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory clientFactory;
        public AccountController(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(String Email , string Password)
        {
            var client = clientFactory.CreateClient("ECommerceAPI");
            var loginData = new { Email = Email, Password = Password };
            var content = new StringContent(
               JsonSerializer.Serialize(loginData),
               Encoding.UTF8,
               "application/json"
               );
            var response = await client.PostAsync("User/login", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();

               
                HttpContext.Session.SetString("JWToken", result);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid email or password.";
            return View();
        }
        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }
    }
}
