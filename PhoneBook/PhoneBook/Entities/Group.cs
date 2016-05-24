using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Entities
{
    public class Group:BaseEntity
    {
        [Unique]
        public string GroupName { get; set; }
        public string ImageURI { get; set; }

        [Ignore,ManyToMany(typeof(ContactGroup))]
        public List<Contact> Contacts { get; set; }
    }
}
