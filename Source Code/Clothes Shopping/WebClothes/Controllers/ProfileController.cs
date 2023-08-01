using DAOLibrary.Repository.Interface;
using DTOLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using WebClothes.Models;

namespace WebClothes.Controllers
{
    public class ProfileController : Controller
    {
		private readonly IUserRepository _userRepository;
		private readonly IWebHostEnvironment _webHostEnvironment;
		public ProfileController(IUserRepository userRepository, IWebHostEnvironment webHostEnvironment)
		{
			_userRepository = userRepository;
			_webHostEnvironment = webHostEnvironment;
		}

		[HttpGet]
		public IActionResult Index()
		{
			// Lấy thông tin người dùng hiện tại từ session hoặc cơ sở dữ liệu
			User user = GetCurrentLoggedInUser(); // Hàm này cần được triển khai để lấy thông tin người dùng hiện tại
            bool isLoggedIn = (user != null);
            ViewBag.IsLoggedIn = isLoggedIn;
            if (!isLoggedIn)
			{
				return RedirectToAction("Index", "Login");
			}
			else
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
                // Tính phần trăm hoàn thành dựa trên các thuộc tính đã điền
                int totalFields = 7; // Tổng số trường thông tin trong hồ sơ
                int completedFields = 0; // Số trường thông tin đã hoàn thành

                // Kiểm tra từng trường thông tin và tăng completedFields nếu trường đó đã được điền
                if (!string.IsNullOrEmpty(model.FullName))
                    completedFields++;
                if (!string.IsNullOrEmpty(model.Email))
                    completedFields++;
                if (!string.IsNullOrEmpty(model.Avatar))
                    completedFields++;
                if (!string.IsNullOrEmpty(model.Address))
                    completedFields++;
                if (model.Birthday.HasValue)
                    completedFields++;
                if (model.Gender.HasValue)
                    completedFields++;
                if (!string.IsNullOrEmpty(model.PhoneNumber))
                    completedFields++;

                // Tính phần trăm hoàn thành
                int completionPercentage = (int)((float)completedFields / totalFields * 100);

                // Lưu giá trị phần trăm hoàn thành vào ProfileModel
                model.CompletionPercentage = completionPercentage;
                return View(model);
            }
            
		}

		[HttpPost]
		public IActionResult UpdateProfile(ProfileModel model, IFormFile avatarFile)
		{


			var check = _userRepository.GetUser(model.Email);
			if (check != null && check.Email != model.Email)
			{
				ModelState.AddModelError("Email", "Email đã tồn tại.");
				ViewBag.EmailExistsError = "Email đã tồn tại.";
				return View(model);
			}

			User user = GetCurrentLoggedInUser();

			if (user == null)
			{
				return RedirectToAction("Login", "Account");
			}
			if (avatarFile != null && avatarFile.Length > 0)
			{
				string fileName = avatarFile.FileName;
				string uniqueFileName = "UserID _" + user.UserId + Guid.NewGuid().ToString() + "_" + fileName;

				string webRootPath = _webHostEnvironment.WebRootPath;
				string contentRootPath = _webHostEnvironment.ContentRootPath;
				string path = "";

				string filePath = Path.Combine(webRootPath + "\\Avatars", uniqueFileName);

				// Create the directory if it doesn't exist
				Directory.CreateDirectory(webRootPath);

				using (var fileStream = new FileStream(filePath, FileMode.Create))
				{
					avatarFile.CopyTo(fileStream);
				}

				// Update the model's Avatar property to store the file name or path
				model.Avatar = uniqueFileName;
			}

			user.FullName = model.FullName;
			user.PhoneNumber = model.PhoneNumber;
			user.Email = model.Email;
			user.Address = model.Address;
			user.Gender = model.Gender;
			user.Birthday = model.Birthday;
			user.Avatar = model.Avatar;

			_userRepository.UpdateUser(user);

			TempData["SuccessMessage"] = "Well done! You have successfully updated the information.";


			return RedirectToAction("Index", "Profile");
		}


		[HttpPost]
		public IActionResult UpdatePassword(ChangePasswordModel model)
		{
			// Lấy thông tin người dùng hiện tại từ session hoặc cơ sở dữ liệu
			User user = GetCurrentLoggedInUser();
			if (user == null)
			{
				return RedirectToAction("Index", "Login");
			}

			// Kiểm tra mật khẩu hiện tại
			if (!AuthenticateUser(user.Password, model.oldPassword))
			{
				ModelState.AddModelError("CurrentPassword", "Mật khẩu hiện tại không đúng.");
				ViewBag.EmailExistsError = "Mật khẩu hiện tại không đúng.";
				TempData["FailMessage"] = "Oh snap! The old password is incorrect. Please check back.";
                return RedirectToAction("Index", "Profile");
            }
			else
			{
				if (model.newPassword != model.conFirmPassword)
				{
                    TempData["FailMessage"] = "Oh snap! The new password and confirmation don't match. Please check back.";
                    return RedirectToAction("Index", "Profile");
                }
			}

			// Cập nhật mật khẩu mới
			user.Password = CreateMD5(model.newPassword);

			_userRepository.UpdateUser(user);

			TempData["SuccessMessage"] = "Well done! You have successfully updated your password.";


			return RedirectToAction("Index", "Profile");
		}


		private string CreateMD5(string password)
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

		private bool AuthenticateUser(string oldPassword, string newPassword)
		{
			// Xác thực đăng nhập của người dùng
			// Thực hiện so sánh email và mật khẩu với dữ liệu trong cơ sở dữ liệu
			// Trả về true nếu xác thực thành công, ngược lại trả về false

			// Ví dụ:

			// Mã hóa mật khẩu nhập vào và so sánh với mật khẩu đã lưu
			using (var md5 = MD5.Create())
			{
				byte[] passwordBytes = Encoding.UTF8.GetBytes(newPassword);
				byte[] hashedBytes = md5.ComputeHash(passwordBytes);
				string hashedPassword = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

				return hashedPassword == oldPassword;
			}


			return false;
		}

        [HttpPost]
        public IActionResult DeleteAccount()
        {
            try
            {
                // Lấy thông tin người dùng hiện tại
                User currentUser = GetCurrentLoggedInUser();
                if (currentUser != null)
                {
					if (currentUser.Role == 2)
					{
                        _userRepository.DeleteUser(currentUser.UserId);
                    }
                    // Đăng xuất người dùng (tuỳ thuộc vào cách bạn xử lý đăng xuất)
                    // ...

                    // Chuyển hướng đến trang khác sau khi xóa tài khoản thành công
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Người dùng không tồn tại, xử lý lỗi tại đây
                    // ...
                }
            }
            catch (Exception ex)
            {
            }

            // Nếu không thành công, bạn có thể chuyển hướng hoặc hiển thị thông báo lỗi
            return RedirectToAction("Error");
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
