using PhoneBook.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PhoneBook.Repositories
{
    public class UsersRepository : BaseRepository<User>
    {
        public UsersRepository() : base()
        {
            
        }
        public User GetByUsernameAndPassword(string username, string password)
        {
           return conn.Table<User>().ToList().FirstOrDefault(u => u.Username == username && u.Password == password);
        }
    }
}
