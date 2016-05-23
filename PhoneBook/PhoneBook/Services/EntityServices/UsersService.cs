using PhoneBook.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Services.EntityServices
{
    public class UsersService: BaseService<User>
    {
        public UsersService() : base()
        {

        }
        public User GetByUsernameAndPassword(string username, string password)
        {
            return repo.GetAll().FirstOrDefault(u => u.Username == username && u.Password == password);
        }
    }
}
