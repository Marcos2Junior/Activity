using Domain.Entitys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface ITechnologyRepository : IMainRepository
    {
        Task<Technology> GetTechnologyByIdAsync(int id);
        Task<List<Technology>> GetAllTechnologiesAsync();
    }
}
