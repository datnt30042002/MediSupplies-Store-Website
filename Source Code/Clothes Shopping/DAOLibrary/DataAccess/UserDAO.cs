using DTOLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOLibrary.DataAccess
{
    public class UserDAO
    {
        private static UserDAO instance = null;
        private static readonly object instanceLock = new object();
        public static UserDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new UserDAO();
                    }
                    return instance;
                }
            }
        }

        public User GetUser(string email)
        {
            User user = null;
            try
            {
                using var context = new ClothesShoppingContext();
                user = context.Users
                        .Include(u => u.RoleNavigation)
                        .SingleOrDefault(u => u.Email.Equals(email));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return user;
        }

        public IEnumerable<User> GetUserList()
        {
            var users = new List<User>();
            try
            {
                using var context = new ClothesShoppingContext();
                users = context.Users.Include(u => u.RoleNavigation).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return users;
        }

        public IEnumerable<User> GetActiveAccount()
        {
            var users = new List<User>();
            try
            {
                using var context = new ClothesShoppingContext();
                users = context.Users.Include(u => u.RoleNavigation).Where(u => u.Status == true).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return users;
        }

        public IEnumerable<User> GetInactiveAccount()
        {
            var users = new List<User>();
            try
            {
                using var context = new ClothesShoppingContext();
                users = context.Users.Include(u => u.RoleNavigation).Where(u => u.Status == false).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return users;
        }

        public User GetUserById(int Id)
        {
            User user;
            try
            {
                using var context = new ClothesShoppingContext();
                user = context.Users.Include(u => u.RoleNavigation).FirstOrDefault(u => u.UserId == Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return user;
        }


        public void Update(User user)
        {
            try

            {
                using var context = new ClothesShoppingContext();
                context.Users.Update(user);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void SignUp(User user)
        {
            try
            {
                using var context = new ClothesShoppingContext();
                User _user = context.Users.SingleOrDefault(u => u.Email.ToLower().Equals(user.Email.ToLower()));

                if (_user == null)
                {
                    context.Users.Add(user);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Email is existed! Please try again or login with your email!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public void SetAccountStatus(int id, bool statusVal)
        {
            try
            {
                using var context = new ClothesShoppingContext();
                User user = GetUserById(id);
                user.Status = statusVal;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Remove(int userId)
        {
            try
            {
                User user = GetUserById(userId);
                if (user != null)
                {
                    using var context = new ClothesShoppingContext();
                    context.Users.Remove(user);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The product not already exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void ChangeUserStatus(int userId)
        {
            using (var context = new ClothesShoppingContext())
            {
                var user = context.Users.FirstOrDefault(u => u.UserId == userId);

                if (user != null)
                {
                    bool? newStatus = GetNewStatus(user.Status);
                    user.Status = newStatus;
                    context.SaveChanges();
                    Console.WriteLine($"Đã đổi trạng thái của User có ID {userId} thành {user.Status}");
                }
                else
                {
                    Console.WriteLine($"Không tìm thấy User có ID {userId}");
                }
            }
        }
        private bool? GetNewStatus(bool? currentStatus)
        {
            if (currentStatus == true)
            {
                return false; // Convert "Active" to false
            }
            else if (currentStatus == false)
            {
                return true; // Convert "Inactive" to true
            }
            else
            {
                return null; // Keep "Unknown" as null
            }
        }
    }
}
