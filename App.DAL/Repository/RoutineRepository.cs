using App.DAL.Context;
using App.DAL.Interfaces;
using App.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Repository
{
    public class RoutineRepository : IRoutineRepository
    {
        private readonly AppDbContext _context;
        public RoutineRepository(AppDbContext context) 
        {
            _context = context;
        }
        public bool Add(Routine routine)
        {
            _context.Add(routine);
            return Save();
        }

        public bool Delete(Routine routine)
        {
            _context.Remove(routine);
            return Save();
        }

        public async Task<IEnumerable<Routine>> GetAll()
        {
            return await _context.Routines.ToListAsync();
        }

        public async Task<IEnumerable<Routine>> GetAllRoutinesByCity(string city)
        {
            return await _context.Routines.Where(c => c.Address.City.Contains(city)).ToListAsync();
        }

        public async Task<Routine> GetByIdAsync(int id)
        {
            return await _context.Routines.Include(i => i.Address).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Routine> GetByIdAsyncNoTracking(int id)
        {
            return await _context.Routines.Include(i => i.Address).AsNoTracking().FirstOrDefaultAsync();
        }


        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Routine routine)
        {
            _context.Update(routine);
            return Save();
        }
        public async Task<int> GetCountAsync()
        {
            return await _context.Routines.CountAsync();
        }
    }
}
