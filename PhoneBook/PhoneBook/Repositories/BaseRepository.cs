using PhoneBook.Entities;
using PhoneBook.Repositories;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(BaseRepository<>))]
namespace PhoneBook.Repositories
{
    public abstract class BaseRepository<T> where T:BaseEntity, new()
    {
        protected SQLiteConnection conn;

        public BaseRepository()
        {
            conn = DependencyService.Get<ISQLite>().GetConnection();
        }

        private void Insert(T item)
        {
            conn.Insert(item);
        }

        private void Update(T item)
        {
            conn.Update(item);           
        }

        public void Save(T item)
        {
            if (item.ID!=0)
            {
                Update(item);
            }
            else
            {
                Insert(item);
            }
        }

        public IEnumerable<T> GetAll()
        {
            return conn.Table<T>().ToList();
        }

        public T GetById(int id)
        {
            return conn.Find<T>(id);
        }

        public void Delete(T item)
        {
            conn.Delete(item);
        }
    }
}
