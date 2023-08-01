using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace WebClothes.Controllers
{
    public class LogoutController : Controller
    {
        public IActionResult Index()
        {
            HttpContext.Session.Remove("Email"); 
            return RedirectToAction("Index", "Home");
        }
    }
}
