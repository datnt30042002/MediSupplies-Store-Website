using DAOLibrary.Repository.Interface;
using DAOLibrary.Repository.Object;
using DTOLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using WebClothes.Models;

namespace WebClothes.Controllers
{
    public class SignupController : Controller
    {
        IUserRepository userRepository = null;

        public SignupController()
        {
            userRepository = new UserRepository();
        }

        public HttpContext GetHttpContext()
        {
            return HttpContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.EmailExistsError = null;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(RegisterModel model)
        {
            var check = userRepository.GetUser(model.Email);
            if (check != null)
            {
                ModelState.AddModelError("Email", "This email already exists.");
                ViewBag.EmailExistsError = "This email already exists.";
                return View(model);
            }

            // Mã hóa mật khẩu bằng MD5
            string hashedPassword = MD5Create(model.Password);
            User user = new User()
            {
                FullName = model.FullName,
                Email = model.Email,
                Password = hashedPassword,
                Status = true,
                Role = 2,
                Avatar = "UserID _6ed2d8bf8-0529-4f4c-ad13-b1d3b1547a88_Sample_User_Icon.png"
            };
            userRepository.SignUp(user);
            HttpContext.Session.SetString("Email", model.Email);

            TempData["SuccessMessage"] = "Register Successfully.";
            return RedirectToAction("Index", "Signup");
        }

        public string MD5Create(string password)
        {
            string hashedPassword;
            using (var md5 = MD5.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashedBytes = md5.ComputeHash(passwordBytes);
                hashedPassword = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
            return hashedPassword;
        }

        
    }
}
