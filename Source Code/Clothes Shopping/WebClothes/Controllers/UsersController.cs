using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using DTOLibrary.Models;
using WebClothes.Models;
using DAOLibrary.Repository.Interface;
using System.Linq;
using Microsoft.AspNetCore.Identity;


namespace WebClothes.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ClothesShoppingContext _context;

        public UsersController(IUserRepository userRepository, ClothesShoppingContext context)
        {
            _userRepository = userRepository;
            _context = context;
        }

        // GET: Users
        public IActionResult Index(int page = 1)
        {
            User user = GetCurrentLoggedInUser();
            bool isLoggedIn = (user != null);
            ViewBag.IsLoggedIn = isLoggedIn;
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                int usersPerPage = 7;
                var allUsers = _userRepository.GetUserList().ToList();
                int totalUsers = allUsers.Count;
                int totalPages = (int)Math.Ceiling((double)totalUsers / usersPerPage);

                var pagedUsers = GetUserList(page, usersPerPage);
                var profileModel = new ProfileModel
                {
                    Users = pagedUsers,
                    CurrentPage = page,
                    TotalPages = totalPages
                };
                return View(profileModel);
            }
        }




        private List<User> GetUserList(int page = 1, int usersPerPage = 7)
        {
            var users = _userRepository.GetUserList();
            var pagedUsers = users.Skip((page - 1) * usersPerPage).Take(usersPerPage).ToList();
            return pagedUsers;
        }
        [HttpPost]
        public IActionResult ChangeStatus(int userId)
        {
            var user = _userRepository.GetUserById(userId);

            if (user != null)
            {
                _userRepository.ChangeUserStatus(userId); // Use the injected UserManager here
            }

            return RedirectToAction("Index");
        }
        private User GetCurrentLoggedInUser()
        {

            string email = HttpContext.Session.GetString("Email");
            if (!string.IsNullOrEmpty(email))
            {
                return _userRepository.GetUser(email);
            }
            return null;
        }
    }

}
