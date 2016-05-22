using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using PhoneBook.Entities;

namespace PhoneBook.Droid
{
    public class DBRepository
    {
        public void CreateDB()
        {
            SQLiteConnection conn = new SQLite_Android().GetConnection();
            conn.CreateTable<Phone>();
            conn.CreateTable<User>();
            conn.CreateTable<Contact>();
            
        }
    }
}
