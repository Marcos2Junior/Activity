using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IMainRepository
    {
        Task<bool> AddAsync<T>(T entity) where T : class;
        Task<bool> RemoveAsync<T>(T entity) where T : class;
        Task<bool> UpdateAsync<T>(T entity) where T : class;
    }
}
