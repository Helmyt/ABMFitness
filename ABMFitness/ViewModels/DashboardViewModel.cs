using App.DAL.Models;

namespace App.PL.ViewModels
{
    public class DashboardViewModel
    {
        public List<Routine> Routines { get; set; }
        public List<Club> Clubs { get; set; }
    }
}
