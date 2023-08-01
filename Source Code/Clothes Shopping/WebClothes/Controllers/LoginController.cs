using DAOLibrary.Repository.Interface;
using DAOLibrary.Repository.Object;
using DTOLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using WebClothes.Models;

namespace WebClothes.Controllers
{
    public class LoginController : Controller
    {
        IUserRepository userRepository = null;

        public LoginController()
        {
            userRepository = new UserRepository();
        }
        public IActionResult Index()
        {
            return View();
        }
        public HttpContext GetHttpContext()
        {
            return HttpContext;
        }

        [HttpPost]
        public IActionResult Index(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                if (AuthenticateUser(model.Email, model.Password))
                {
                    User user = userRepository.GetUser(model.Email);
                    if (user != null)
                    {
                        if ((bool)user.Status)
                        {
                            // Người dùng có trạng thái = 1 (kích hoạt)
                            HttpContext.Session.SetString("Email", model.Email);
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            // Người dùng có trạng thái = 0 (bị khóa)
                            ModelState.AddModelError(string.Empty, "Your account has been locked. Please contact the administrator.");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "The email or password is incorrect.");
                }
            }
            return View(model);
        }

        private bool AuthenticateUser(string email, string password)
        {

            User user = userRepository.GetUser(email);
            if (user != null)
            {
                using (var md5 = MD5.Create())
                {
                    byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                    byte[] hashedBytes = md5.ComputeHash(passwordBytes);
                    string hashedPassword = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                    return hashedPassword == user.Password;
                }
            }
            return false;
        }

        
    }
}
