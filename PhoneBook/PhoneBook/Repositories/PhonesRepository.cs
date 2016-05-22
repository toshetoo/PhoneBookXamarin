using PhoneBook.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Repositories
{
    public class PhonesRepository:BaseRepository<Phone>
    {
        public IEnumerable<Phone> GetPhonesByContactID(int id)
        {
            return conn.Table<Phone>().ToList().Where(p => p.ContactID == id);
        }
    }
}
