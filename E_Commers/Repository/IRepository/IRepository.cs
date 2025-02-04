﻿ using E_Commers.Models;
using System.Linq.Expressions;

namespace E_Commers.Repository.IRepository
{
    public interface IRepository<T>where T : class
    {
        IQueryable<T> GetAll(Expression<Func<T, object>>[]? inculdeProp = null, Expression<Func<T, bool>>? expression = null, bool tracked = true);


        T? GetOne(Expression<Func<T, object>>[]? inculdeProp = null, Expression<Func<T, bool>>? expression = null, bool tracked = true);

        void Add(T category);

        void Edit(T category);

        void Delete(T category);

        void Commit();
    }
}
