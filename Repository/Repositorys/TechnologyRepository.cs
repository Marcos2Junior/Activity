using Domain.Entitys;
using Microsoft.EntityFrameworkCore;
using Repository.Context;
using Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Repositorys
{
    public class TechnologyRepository : MainRepository, ITechnologyRepository
    {
        public TechnologyRepository(ActivityDbContext context) : base(context)
        {
        }

        public async Task<List<Technology>> GetAllTechnologiesAsync()
            => await _context.Technologies.OrderBy(x => x.Name).ToListAsync();

        public async Task<Technology> GetTechnologyByIdAsync(int id)
            => await _context.Technologies.FirstOrDefaultAsync(x => x.Id == id);
    }
}
