using E_Commers.Data;
using E_Commers.Models;
using E_Commers.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace E_Commers.Repository
{
    public class Repository<T> : IRepository<T> where T:class
    {
        ApplicationDbContext dbContext; /*= new ApplicationDbContext()*/
        DbSet<T> dbSet; 
        public Repository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            dbSet = dbContext.Set<T>();
        }

        // CRUD
        public IQueryable<T> GetAll(Expression<Func<T, object>>[]? inculdeProp = null, Expression<Func<T, bool>>? expression = null, bool tracked = true)
        {
            IQueryable<T> query = dbSet;
         

            if (inculdeProp != null)
            {
                foreach(var item in inculdeProp)
                {
                    query = query.Include(item);
                }
                
            }
            if (expression != null)
            {
                query= query.Where(expression);
            }
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            return query; 

        }

        public T? GetOne(Expression<Func<T, object>>[]? inculdeProp = null, Expression<Func<T, bool>>? expression = null, bool tracked = true)
        {
            return GetAll(inculdeProp, expression, tracked).FirstOrDefault();
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }
        public void Edit(T entity)
        {
            dbSet.Update(entity);
        }
        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        } 
        public void Commit()
        {
            dbContext.SaveChanges();
        }
    }
}
