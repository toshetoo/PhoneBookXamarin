using PhoneBook.Entities;
using PhoneBook.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Services.EntityServices
{
    public abstract class BaseService<T> where T : BaseEntity, new()
    {
        protected readonly BaseRepository<T> repo;

        public BaseService()
        {
            this.repo = new BaseRepository<T>();
        }

        public T GetById(int id)
        {
            return this.repo.GetById(id);
        }

        public IEnumerable<T> GetAll()
        {
            return this.repo.GetAll();
        }

        public void Save(T item)
        {
            this.repo.Save(item);
        }

        public void Delete(T item)
        {
            this.repo.Delete(item);
        }
    }
}
