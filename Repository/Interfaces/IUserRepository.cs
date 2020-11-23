using Domain.Entitys;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IUserRepository : IMainRepository
    {
        Task<User> GetUserByIdAsync(int id);
        Task<User> VerifyAcessAsync(string password, string email);
    }
}
