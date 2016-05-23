using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using PhoneBook.Entities;
using PhoneBook.Repositories;

namespace PhoneBook.Droid
{
    public class DBRepository
    {
        private void CreateDB()
        {
            SQLiteConnection conn = new SQLite_Android().GetConnection();
            conn.CreateTable<Phone>();
            conn.CreateTable<User>();
            conn.CreateTable<Contact>();
            conn.CreateTable<UserGroup>();
        }

        public void DropDB()
        {
            
            SQLiteConnection conn = new SQLite_Android().GetConnection();
            string dropUsersQuery = "DROP TABLE User";
            SQLiteCommand dropUsersCommand = conn.CreateCommand(dropUsersQuery);
            dropUsersCommand.ExecuteNonQuery();

            string dropContactsQuery = "DROP TABLE Contact";
            SQLiteCommand dropContactsCommand = conn.CreateCommand(dropContactsQuery);
            dropContactsCommand.ExecuteNonQuery();

            string dropPhonesQuery = "DROP TABLE Phone";
            SQLiteCommand dropPhonesCommand = conn.CreateCommand(dropPhonesQuery);
            dropPhonesCommand.ExecuteNonQuery();

            string dropGroupsTable = "DROP TABLE UserGroup";
            SQLiteCommand dropGroupsCommand = conn.CreateCommand(dropGroupsTable);
            dropGroupsCommand.ExecuteNonQuery();
        }

        public void InitDB()
        {
            CreateDB();
            UsersRepository usersRepo = new UsersRepository();
            User u = new User();
            u.FirstName = "Admin";
            u.LastName = "Adminov";
            u.Username = "admin";
            u.Password = "admin";
            u.Email = "admin@phonebook.com";

            usersRepo.Save(u);
        }
    }
}
