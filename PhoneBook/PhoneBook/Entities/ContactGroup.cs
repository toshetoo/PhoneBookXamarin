using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Entities
{
    public class ContactGroup : BaseEntity
    {
        [ForeignKey(typeof(Contact))]
        public int ContactID { get; set; }

        [ForeignKey(typeof(Group))]
        public int GroupID { get; set; }
    }
}
