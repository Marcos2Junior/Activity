using Microsoft.EntityFrameworkCore;
using Repository.Context;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repository.Repositorys
{
    public class MainRepository : IMainRepository
    {
        protected readonly ActivityDbContext _context;

        public MainRepository(ActivityDbContext context)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        private async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;

        public async Task<bool> AddAsync<T>(T entity) where T : class
        {
            await _context.AddAsync(entity);

            return await SaveChangesAsync();
        }

        public async Task<bool> RemoveAsync<T>(T entity) where T : class
        {
            _context.Remove(entity);
            return await SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync<T>(T entity) where T : class
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _context.Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
            }
            else
            {
                _context.Update(entity);
            }

            return await SaveChangesAsync();
        }

        public async Task<T> GetWhereFirstEntityAsync<T>(Expression<Func<T, bool>> where) where T : class
        {
            return await _context.Set<T>().FirstOrDefaultAsync(where);

        }

        public async Task<List<T>> GetWhereAllEntityAsync<T>(Expression<Func<T, bool>> where) where T : class
        {
            return await _context.Set<T>().Where(where).ToListAsync();
        }
    }
}
