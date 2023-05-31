using App.DAL.Models;

namespace App.DAL.Interfaces
{
    public interface IDashboardRepository
    {
        Task<List<Routine>> GetAllUserRoutines();
        Task<List<Club>> GetAllUserClubs();
        Task<AppUser> GetUserById(string id);
        Task<AppUser> GetByIdNoTracking(string id);
        bool Update(AppUser user);
        bool Save();
    }
}
