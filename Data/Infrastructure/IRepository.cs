﻿using System.Linq.Expressions;

namespace HotelServer.Data.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        Task Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        void DeleteMulti(Expression<Func<T, bool>> where);

        T GetSingleById(string id);

        T GetSingleByCondition(Expression<Func<T, bool>> expression, string[] includes = null);

        IQueryable<T> GetAll(String[] includes = null);
        IQueryable<T> GetMulti(Expression<Func<T, bool>> predicate, string[] includes = null);
        IQueryable<T> GetMultiPaging(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50, string[] includes = null);

        int Count(Expression<Func<T, bool>> where);

        bool CheckContains(Expression<Func<T, bool>> predicate);

    }
}
