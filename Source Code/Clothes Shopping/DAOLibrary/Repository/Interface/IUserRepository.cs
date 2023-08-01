using DTOLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOLibrary.Repository.Interface
{
    public interface IUserRepository
    {
        public User GetUser(string email);


        IEnumerable<User> GetUserList();

        IEnumerable<User> GetActiveUser();

        IEnumerable<User> GetInactiveUser();

        User GetUserById(int id);

        void UpdateUser(User user);
        void DeleteUser(int userId);

        void SetAccountStatus(int id, bool status);

        void ChangeUserStatus(int id);

        public void SignUp(User user);
    }
}
