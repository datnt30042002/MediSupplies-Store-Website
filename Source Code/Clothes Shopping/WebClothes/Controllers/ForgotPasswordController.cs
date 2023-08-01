using DAOLibrary.Repository.Interface;
using DAOLibrary.Repository.Object;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Security.Cryptography;
using System.Text;
using WebClothes.Models;

namespace WebClothes.Controllers
{
    public class ForgotPasswordController : Controller
    {
        IUserRepository userRepository = null;

        public ForgotPasswordController()
        {
            userRepository = new UserRepository();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(LoginModel model)
        {

            var check = userRepository.GetUser(model.Email);
            if (check == null)
            {
                ModelState.AddModelError("Email", "Email không tồn tại.");

                return View(model);
            }

            var subject = "https://localhost:7051/ForgotPassword/ResetPassword/" + check.UserId;

            bool _check = sendMail(subject, check.Email);
            if (_check == true)
            {
                TempData["SuccessMessage"] = "Password reset link sent to email address - " + check.Email + ".";
            }
            else
            {
                TempData["FailMessage"] = "Hệ thống không gửi được email. Hãy kiểm tra lại kết nối mạng và các vấn đề khác.";
            }
            


            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string id)
        {
            ChangePasswordModel changePasswordModel = new ChangePasswordModel()
            {
                userID = id
            };

            return View(changePasswordModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResetPassword(ChangePasswordModel model)
        {
            var user = userRepository.GetUserById(int.Parse(model.userID));
            if (model.newPassword == model.conFirmPassword)
            {
                user.Password = MD5Create(model.newPassword);
                ViewBag.SuccessMessage = "Mật khẩu đã được đặt lại thành công.";
                TempData["SuccessMessage"] = "Well done! You have successfully changed your password.";
                userRepository.UpdateUser(user);
            }
            else
            {
                ViewBag.SuccessMessage = "Mật khẩu đã được đặt lại thành công.";
                TempData["FailMessage"] = "Oh snap! The new password and confirmation don't match. Please check back.";
            }
            return RedirectToAction("ResetPassword", "ForgotPassword");
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

        public bool sendMail(string Subject, string userEmail)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Doctris - Doctor Appointment Booking System", "nguyentien18011978@gmail.com"));
                message.To.Add(new MailboxAddress("Người nhận", userEmail));
                message.Subject = "Request password recovery";
                message.Body = new TextPart("plain")
                {
                    Text = "To reset your password, please click on the following link: " + Subject
                };

                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    client.Authenticate("nguyentien18011978@gmail.com", "jlvpbyfzboeuklbn");
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
