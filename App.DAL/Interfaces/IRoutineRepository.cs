using App.DAL.Models;

namespace App.DAL.Interfaces
{
    public interface IRoutineRepository
    {
        Task<IEnumerable<Routine>> GetAll();
        Task<Routine> GetByIdAsync(int id);
        Task<Routine> GetByIdAsyncNoTracking(int id);
        Task<IEnumerable<Routine>> GetAllRoutinesByCity(string city);
        bool Add(Routine routine);
        bool Update(Routine routine);
        bool Delete(Routine routine);
        bool Save();
    }
}