using PhoneBook.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Services.EntityServices
{
    public class PhonesService : BaseService<Phone>
    {
        public PhonesService() : base()
        {

        }

        public IEnumerable<Phone> GetPhonesByContactID(int id)
        {
            return repo.GetAll().Where(p => p.ContactID == id);
        }
    }
}
