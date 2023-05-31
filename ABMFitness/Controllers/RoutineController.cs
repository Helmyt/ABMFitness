using App.DAL.Context;
using App.DAL.Interfaces;
using App.DAL.Models;
using App.DAL.Repository;
using App.BLL.Helpers;
using App.BLL.Services;
using App.PL.ViewModels;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace App.PL.Controllers
{
    public class RoutineController : Controller
    {
        private readonly IRoutineRepository _routineRepository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RoutineController(IRoutineRepository routineRepository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor) 
        {
            _routineRepository = routineRepository;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Routine> routines = await _routineRepository.GetAll();
            return View(routines);
        }
        public async Task<IActionResult> Detail(int id)
        {
            Routine routine = await _routineRepository.GetByIdAsync(id);
            return View(routine);
        }
        public IActionResult Create()
        {
            var curUserID = _httpContextAccessor.HttpContext?.User.GetUserId();
            var createRoutineViewModel = new CreateRoutineViewModel { AppUserId = curUserID };
            return View(createRoutineViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoutineViewModel routineVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(routineVM.Image);

                var routine = new Routine
                {
                    Title = routineVM.Title,
                    Description = routineVM.Description,
                    Image = result.Url.ToString(),
                    AppUserId = routineVM.AppUserId,
                    Address = new Address
                    {
                        Street = routineVM.Address.Street,
                        City = routineVM.Address.City,
                        State = routineVM.Address.State,
                    }
                };
                _routineRepository.Add(routine);
                return RedirectToAction("Index");
            }
            else
            {
                var errors1 = ModelState.Where(x => x.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors });
                var errors2 = ModelState.Values.SelectMany(v => v.Errors);
                ModelState.AddModelError("", "Photo upload failed");
            }
            return View(routineVM);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var routine = await _routineRepository.GetByIdAsync(id);
            if (routine == null) return View("Error");
            var clubVM = new EditRoutineViewModel
            {
                Title = routine.Title,
                Description = routine.Description,
                AddressId = (int)routine.AddressId,
                Address = routine.Address,
                URL = routine.Image,
                RoutineCategory = routine.RoutineCategory
            };
            return View(clubVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditRoutineViewModel routineVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("Edit", routineVM);
            }

            var userRoutine = await _routineRepository.GetByIdAsyncNoTracking(id);

            if (userRoutine != null)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(userRoutine.Image);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(routineVM);
                }
                var photoResult = await _photoService.AddPhotoAsync(routineVM.Image);


                var routine = new Routine
                {
                    Id = id,
                    Title = routineVM.Title,
                    Description = routineVM.Description,
                    Image = photoResult.Url.ToString(),
                    AddressId = routineVM.AddressId,
                    Address = routineVM.Address,
                };

                _routineRepository.Update(routine);

                return RedirectToAction("Index");
            }
            else
            {
                return View(routineVM);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var clubDetails = await _routineRepository.GetByIdAsync(id);
            if (clubDetails == null) return View("Error");
            return View(clubDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var raceDetails = await _routineRepository.GetByIdAsync(id);
            if (raceDetails == null) return View("Error");

            _routineRepository.Delete(raceDetails);
            return RedirectToAction("Index");
        }
    }
}
