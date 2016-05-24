using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Entities
{
    public class Contact : BaseEntity
    {
        
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageURI { get; set; }
        [Unique]
        public string Email { get; set; }

        [Ignore,ManyToMany(typeof(ContactGroup))]
        public List<Group> Groups { get; set; }

        [Ignore]
        public List<Phone> Phones { get; set; }
    }
}
