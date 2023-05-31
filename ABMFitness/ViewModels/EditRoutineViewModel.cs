using App.BLL.DataEnums;
using App.DAL.Models;

namespace App.PL.ViewModels
{
    public class EditRoutineViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public string? URL { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public RoutineCategory RoutineCategory { get; set; }
    }
}
