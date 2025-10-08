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
            var response = await client.PostAsync("Users/login", content);


            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();

               
                HttpContext.Session.SetString("JWToken", result);

                return RedirectToAction("Index", "Products");
            }

            ViewBag.Error = "Invalid email or password.";
            return View();
        }
        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Signup(string Name, string Email, string PasswordHash)
        {
            var client = clientFactory.CreateClient("ECommerceAPI");

            var signupData = new
            {
                Name = Name,
                Email = Email,
                PasswordHash = PasswordHash,
                Role = "User"
            };

            var content = new StringContent(
                JsonSerializer.Serialize(signupData),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync("Users/register", content);
            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Signup successful! Please login now.";
                return RedirectToAction("Login");
            }

            ViewBag.Error = $"Signup failed: {result}";
            return View();
        }
    }
}
