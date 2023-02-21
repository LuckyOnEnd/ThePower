using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ThePower.Data.Repository.IRepository;
using ThePowerData.Data;

namespace ThePower.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T GetFirst(Expression<Func<T, bool>> id)
        {
            IQueryable<T> values = dbSet;

            values = values.Where(id);

            return values.FirstOrDefault();
        }
        public void Edit(T entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> values = dbSet;
            return values.ToList();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        
    }
}
