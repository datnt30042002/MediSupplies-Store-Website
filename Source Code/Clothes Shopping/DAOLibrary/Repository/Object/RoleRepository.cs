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
    internal class RoleRepository : IRoleRepository
    {
        public IEnumerable<Role> GetRolesList() => RoleDAO.Instance.GetRoleList();
    }
}
