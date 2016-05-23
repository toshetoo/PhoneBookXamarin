using PhoneBook.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Services.EntityServices
{
    public class ContactsService:BaseService<Contact>
    {
        public ContactsService():base()
        {
           
        }

        public IEnumerable<Contact> GetAllByUserID(int id)
        {
            return repo.GetAll().Where(c => c.UserID == id);
        }
    }
}
