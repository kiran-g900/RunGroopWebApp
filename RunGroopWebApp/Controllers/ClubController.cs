using LayerDAL.Entities;
using LayerDAL.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using LayerDAL.ViewModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Net.Mime.MediaTypeNames;
using LayerDAL;

namespace RunGroopWebApp.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubRepository _context;
        private readonly IHttpContextAccessor _contextAccessor;
        public ClubController(IClubRepository context, IHttpContextAccessor httpContextAccessor) 
        {
            this._context = context;
            this._contextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Club> clubs = await _context.GetClubs();
            return View(clubs);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Club club = await _context.GetClub(id);
            return View(club);
        }

        public IActionResult Create()
        {
            var curUserId = _contextAccessor.HttpContext.User.GetUserId();
            var createClubViewModel = new CreateClubViewModel
            {
                AppUserId = curUserId
            };
            return View(createClubViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel vmClub, IFormFile Image)
        {
            string fileName = "";            
            ModelState.Remove("Image");
            if (!ModelState.IsValid)
            {
                return View(vmClub);
            }
            if(Image != null)
            {
                fileName = vmClub.Title + "_" + Path.GetExtension(Image.FileName);
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\images",fileName);
                var stream = new FileStream(uploadPath,FileMode.Create);
                await Image.CopyToAsync(stream);
                stream.Close();
                vmClub.Image = "../../images/" + fileName;
            }

            var club = new Club
            {
                Title = vmClub.Title,
                Description = vmClub.Description,
                Image = vmClub.Image,
                AppUserId = vmClub.AppUserId,
                Address = new Address
                {
                    Street = vmClub.Address.Street,
                    City = vmClub.Address.City,
                    State = vmClub.Address.State
                }
            };

            bool isAdded = _context.Add(club);
            if(isAdded)
            {
                return RedirectToAction("Index");
            }
            return View(club);
        }
    
        public async Task<IActionResult> Edit(int id)
        {
            var club = await _context.GetClub(id);
            if(club == null) { return View("Error"); }
            var clubVm = new EditClubViewModel
            {
                Title = club.Title,
                Description = club.Description,
                AddressId = club.AddressId,
                Address = club.Address,
                Image = club.Image,
                ClubCategory = club.ClubCategory
            };
            return View(clubVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubViewModel clubVM, IFormFile Image)
        {
            string fileName = "";

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("Edit", clubVM);
            }

            var userClub = await _context.GetClubNoTracking(id);
            if(userClub != null) 
            {
                if (Image != null)
                {
                    fileName = clubVM.Title +  Path.GetExtension(Image.FileName);
                    string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);
                    var stream = new FileStream(uploadPath, FileMode.Create);
                    await Image.CopyToAsync(stream);
                    stream.Close();
                    clubVM.Image = "../../images/" + fileName;
                }

                var club = new Club
                {
                    Id = id,
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    AddressId = clubVM.AddressId,
                    Address = clubVM.Address,
                    Image = clubVM.Image
                };

                _context.Update(club);
                return RedirectToAction("Index");
            }
            else
            {
                return View(clubVM);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var clubDetails = await _context.GetClub(id);
            if (clubDetails == null) { return View("Error"); }
            return View(clubDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var clubDetails = await _context.GetClub(id);
            if (clubDetails == null) { return View("Error"); }

            _context.Delete(clubDetails);
            return RedirectToAction("Index");
        }
    }
}
