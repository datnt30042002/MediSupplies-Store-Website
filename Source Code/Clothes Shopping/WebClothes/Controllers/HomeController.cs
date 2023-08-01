using DAOLibrary.Repository.Interface;
using DAOLibrary.Repository.Object;
using DTOLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebClothes.Models;

namespace WebClothes.Controllers
{
    public class HomeController : Controller
    {
        IUserRepository userRepository = null;

        private readonly ILogger<HomeController> _logger;

        // context 
        private readonly ClothesShoppingContext _context = new();

        public HomeController(ILogger<HomeController> logger)
        {
            userRepository = new UserRepository();
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<Product> product = _context.Products.ToList();
            List<Category> category = _context.Categories.ToList();

            User user = GetCurrentLoggedInUser();
            bool isLoggedIn = (user != null);
            ViewBag.IsLoggedIn = isLoggedIn;
            if (isLoggedIn)
            {
                ProfileModel model = new ProfileModel
                {
                    FullName = user.FullName,
                    Email = user.Email,
                    Avatar = user.Avatar,
                    Address = user.Address,
                    Birthday = user.Birthday,
                    Gender = user.Gender,
                    PhoneNumber = user.PhoneNumber,
                    RoleName = user.RoleNavigation?.RoleName
                };
                ViewData["Product"] = product;
                ViewData["Category"] = category;
                return View(model);
            }

            // list product 
            //List<Product> product = _context.Products.ToList();
            ViewData["Product"] = product;
            ViewData["Category"] = category;
            // end 

            // view to Home 
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

        private User GetCurrentLoggedInUser()
        {
            string email = HttpContext.Session.GetString("Email");
            if (!string.IsNullOrEmpty(email))
            {
                return userRepository.GetUser(email);
            }
            return null;
        }
    }
}