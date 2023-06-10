using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using ShopOnline.Models;
using ShopOnline.Models.EF;

namespace ShopOnline.Data
{
    public class DataAccessLayer
    {
        private readonly ApplicationDbContext _db;

        public DataAccessLayer()
        {
            _db = new ApplicationDbContext();
        }

        public List<T> GetAll<T>() where T : class
        {
            return _db.Set<T>().ToList();
        }
        public List<T> GetAllWithInclude<T>(params Expression<Func<T, object>>[] includes) where T : class
        {
            IQueryable<T> query = _db.Set<T>();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query.ToList();
        }

        public T GetById<T>(int id) where T : class
        {
            return _db.Set<T>().Find(id);
        }

        public T GetById<T>(string id) where T : class
        {
            return _db.Set<T>().Find(id);
        }

        public void Add<T>(T entity) where T : class
        {
            _db.Set<T>().Add(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _db.Entry(entity).State = EntityState.Modified;
        }

        public void Delete<T>(T entity) where T : class
        {
            _db.Set<T>().Remove(entity);
        }

        public void DeleteAll<T>(IEnumerable<T> entities) where T : class
        {
            _db.Set<T>().RemoveRange(entities);
        }

        public void Clear<T>() where T : class
        {
            var entities = _db.Set<T>().ToList();
            _db.Set<T>().RemoveRange(entities);
        }

        public int GetCount<T>() where T : class
        {
            return _db.Set<T>().Count();
        }

        public List<T> GetFiltered<T>(Func<T, bool> predicate) where T : class
        {
            return _db.Set<T>().Where(predicate).ToList();
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }
    }
}
