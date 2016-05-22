using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Entities
{
    public class Phone : BaseEntity
    {
        public int ContactID { get; set; }
        public string Type { get; set; }
        public string Number { get; set; }

        public enum PhoneType
        {
            Home, Mobile, Fax
        }
    }
}
