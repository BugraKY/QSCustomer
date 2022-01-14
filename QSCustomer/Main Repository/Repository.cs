using Microsoft.EntityFrameworkCore;
using QSCustomer.Data;
using QSCustomer.IMainRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace QSCustomer.MainRepository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbApp;
        private readonly SecondDbContext _dbSec;
        internal DbSet<T> dbSet;
        
        public Repository(ApplicationDbContext dbApp)
        {
            _dbApp = dbApp;
            this.dbSet = _dbApp.Set<T>();
        }
        public Repository(SecondDbContext dbSec)
        {
            _dbSec = dbSec;
            this.dbSet = _dbSec.Set<T>();
        }
        /// <summary>
        /// Generic Added Entity
        /// </summary>
        /// <param name="entity"></param>
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(int id)
        {
            return dbSet.Find(id);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var item in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var item in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return query.FirstOrDefault();
        }

        public void Remove(int id)
        {
            T entitiy = dbSet.Find(id);
            Remove(entitiy);
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        /// <summary>
        /// Remove Range Multiple Entity
        /// </summary>
        /// <param name="entity"></param>
        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }
}
