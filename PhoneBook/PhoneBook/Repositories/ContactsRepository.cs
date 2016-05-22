using PhoneBook.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Repositories
{
    public class ContactsRepository: BaseRepository<Contact>
    {
        public IEnumerable<Contact> GetAllByUserID(int id)
        {
            return conn.Table<Contact>().ToList().Where(c => c.UserID == id);
        }
    }
}
