using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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

        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        public void ReduceProductQuantity(int productId, int quantity)
        {
            var product = _db.Products.FirstOrDefault(p => p.Id == productId);
            if (product != null && product.Quantity >= quantity)
            {
                product.Quantity -= quantity;
            }
        }
    }
}
