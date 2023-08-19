using LayerDAL;
using LayerDAL.Entities;
using LayerDAL.Repository.Interfaces;
using LayerDAL.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RunGroopWebApp.Controllers
{
    public class RaceController : Controller
    {
        private readonly IRaceRepository _repository;
        private readonly IHttpContextAccessor _contextAccessor;
        public RaceController(IRaceRepository repository, IHttpContextAccessor contextAccessor)
        {
            _repository = repository;
            _contextAccessor = contextAccessor;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Race> races = await _repository.GetRaces();
            return View(races);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Race race = await _repository.GetRace(id);
            return View(race);
        }

        public IActionResult Create()
        {
            var curUserId = _contextAccessor.HttpContext.User.GetUserId();
            var createRaceViewModel = new CreateRaceViewModel { 
                AppUserId = curUserId,
            };
            return View(createRaceViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRaceViewModel vmRace, IFormFile Image)
        {
            string fileName = "";
            ModelState.Remove("Image");
            if (!ModelState.IsValid)
            {
                return View(vmRace);
            }
            if(Image != null)
            {
                fileName = vmRace.Title + "_" + Path.GetExtension(Image.FileName);
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\Race", fileName);
                var stream = new FileStream(uploadPath, FileMode.Create);
                await Image.CopyToAsync(stream);
                vmRace.Image = "../../images/Race/" + fileName;
            }

            var race = new Race
            {
                Title = vmRace.Title,
                Description = vmRace.Description,
                Image = vmRace.Image,
                AppUserId = vmRace.AppUserId,
                Address = new Address 
                { 
                    City = vmRace.Address.City,
                    State = vmRace.Address.State,
                    Street = vmRace.Address.Street
                }
            };

            bool isAdded = _repository.Add(race);
            if (isAdded)
            {
                return RedirectToAction("Index");
            }
            return View(vmRace);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var race = await _repository.GetRace(id);
            if (race == null) { return View("Error"); }
            var raceVM = new EditRaceViewModel
            {
                Title = race.Title,
                Description = race.Description,
                AddressId = race.AddressId,
                Address = race.Address,
                Image = race.Image,
                RaceCategory = race.RaceCategory
            };
            return View(raceVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditRaceViewModel raceVM, IFormFile Image)
        {
            string fileName = "";

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("Edit", raceVM);
            }

            var userRace = await _repository.GetRaceNoTracking(id);
            if (userRace != null)
            {
                if (Image != null)
                {
                    fileName = raceVM.Title + Path.GetExtension(Image.FileName);
                    string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\Race", fileName);
                    var stream = new FileStream(uploadPath, FileMode.Create);
                    await Image.CopyToAsync(stream);
                    stream.Close();
                    raceVM.Image = "../../images/Race/" + fileName;
                }

                var race = new Race
                {
                    Id = id,
                    Title = raceVM.Title,
                    Description = raceVM.Description,
                    AddressId = raceVM.AddressId,
                    Address = raceVM.Address,
                    Image = raceVM.Image,
                    RaceCategory = raceVM.RaceCategory
                };

                _repository.Update(race);
                return RedirectToAction("Index");
            }
            else
            {
                return View(raceVM);
            }            
        }

        public async Task<IActionResult> Delete(int id)
        {
            var RaceDetails = await _repository.GetRace(id);
            if(RaceDetails == null) return View("Error");
            return View(RaceDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteRace(int id)
        {
            var RaceDetails = await _repository.GetRace(id);
            if (RaceDetails == null) return View("Error");
            _repository.Delete(RaceDetails);
            return RedirectToAction("Index");
        }
    }
}
