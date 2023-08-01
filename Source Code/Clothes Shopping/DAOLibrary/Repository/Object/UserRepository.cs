using DAOLibrary.DataAccess;
using DAOLibrary.Repository.Interface;
using DTOLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOLibrary.Repository.Object
{
    public class UserRepository : IUserRepository
    {
        public void DeleteUser(int userId) => UserDAO.Instance.Remove(userId);

        public User GetUser(string email) => UserDAO.Instance.GetUser(email);

        public User GetUserById(int id) => UserDAO.Instance.GetUserById(id);

        public IEnumerable<User> GetUserList() => UserDAO.Instance.GetUserList();

        public void SetAccountStatus(int id, bool status) => UserDAO.Instance.SetAccountStatus(id, status);

        public void UpdateUser(User user) => UserDAO.Instance.Update(user);


        public void SignUp(User user) => UserDAO.Instance.SignUp(user);


        public IEnumerable<User> GetActiveUser() => UserDAO.Instance.GetActiveAccount();


        public IEnumerable<User> GetInactiveUser() => UserDAO.Instance.GetInactiveAccount();

        public void ChangeUserStatus(int userId) => UserDAO.Instance.ChangeUserStatus(userId);
    }
}
