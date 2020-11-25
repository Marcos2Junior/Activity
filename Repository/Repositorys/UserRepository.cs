using Domain.Entitys;
using Microsoft.EntityFrameworkCore;
using Repository.Context;
using Repository.Interfaces;
using System.Threading.Tasks;

namespace Repository.Repositorys
{
    public class UserRepository : MainRepository, IUserRepository
    {
        public UserRepository(ActivityDbContext context) : base(context)
        {
        }

        public async Task<User> GetUserByIdAsync(int idUser)
        => await _context.Users.FirstOrDefaultAsync(x => x.Id == idUser);

        public async Task<User> VerifyAcessAsync(string password, string email)
            => await _context.Users.FirstOrDefaultAsync(x => x.Password == password && x.Email == email);

        public async Task<User> VerifyByEmailAsync(string email)
        => await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
    }
}
