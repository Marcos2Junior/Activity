using Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IMainRepository
    {
        Task<bool> AddAsync<T>(T entity) where T : class;
        Task<bool> RemoveAsync<T>(T entity) where T : class;
        Task<bool> UpdateAsync<T>(T entity) where T : class;
        Task<T> GetWhereFirstEntityAsync<T>(Expression<Func<T, bool>> where) where T : class;
        Task<List<T>> GetWhereAllEntityAsync<T>(Expression<Func<T, bool>> where) where T : class;
    }
}
