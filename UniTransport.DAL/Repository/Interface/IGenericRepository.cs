using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniTransport.DAL.Repository.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<bool> CreateAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
    }
}
