using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniTransport.DAL.Database;
using UniTransport.DAL.Repository.Interface;

namespace UniTransport.DAL.Repository.Implementation
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<bool> CreateAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }

        public virtual async Task<bool> UpdateAsync(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return false;

            try
            {
                _dbSet.Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
