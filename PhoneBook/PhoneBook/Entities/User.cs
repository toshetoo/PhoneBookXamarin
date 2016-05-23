using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Unique]
        public string Email { get; set; }
        [Unique]
        public string Username { get; set; }
        public string Password { get; set; }
        public string ImageURI { get; set; }

        [Ignore]
        public List<Contact> Contacts { get; set; }

        public User():base()
        {

        }
    }
}
