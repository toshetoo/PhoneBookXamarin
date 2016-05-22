using SQLite;
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

        [Ignore]
        public List<Phone> Phones { get; set; }
    }
}
