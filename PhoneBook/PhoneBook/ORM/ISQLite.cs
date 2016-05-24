using SQLite;
using SQLiteNetExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}
