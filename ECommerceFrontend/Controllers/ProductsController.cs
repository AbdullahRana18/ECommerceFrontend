using Microsoft.AspNetCore.Mvc;

namespace ECommerceFrontend.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
