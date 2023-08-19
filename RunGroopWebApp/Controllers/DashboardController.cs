using LayerDAL;
using LayerDAL.Data;
using LayerDAL.Entities;
using LayerDAL.Repository.Interfaces;
using LayerDAL.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace RunGroopWebApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        public DashboardController(IDashboardRepository dashboardRepository, IHttpContextAccessor contextAccessor)
        {
            _dashboardRepository = dashboardRepository;
            _contextAccessor = contextAccessor;
        }
       
        public async Task<IActionResult> Index()
        {
            var userRaces = await _dashboardRepository.GetAllUserRaces();
            var userClubs = await _dashboardRepository.GetAllUserClubs();
            var dashboardViewModel = new DashboardViewModel()
            {
                Races = userRaces,
                Clubs = userClubs
            };
            return View(dashboardViewModel);
        }

        public async Task<IActionResult> EditUserProfile()
        {
            var curUserId = _contextAccessor.HttpContext.User.GetUserId();
            var user = await _dashboardRepository.GetUserById(curUserId);
            if (user == null) 
            {
                return View("Error");
            }
            var editUserViewModel = new EditUserDashBoardViewModel()
            {
                Id = curUserId,
                Pace = user.Peace,
                Mileage = user.Mileage,
                ProfileImageUrl = user.ProfileImageUrl,
                City = user.City,
                State = user.State
            };
            return View(editUserViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditUserDashBoardViewModel editVM, IFormFile Image)
        {
            string fileName = "";
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit profile");
                return View("EditUserProfile", editVM);
            }

            var user = await _dashboardRepository.GetUserByIdNoTracking(editVM.Id);            

            if(user.ProfileImageUrl == "" || user.ProfileImageUrl == null)
            {
                fileName = editVM.Id + "_" + Path.GetExtension(Image.FileName);
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);
                var stream = new FileStream(uploadPath, FileMode.Create);
                await Image.CopyToAsync(stream);
                stream.Close();
                editVM.ProfileImageUrl = "../../images/" + fileName;

                var updatedUser = new AppUser()
                {
                    Peace = editVM.Pace,
                    Mileage = editVM.Mileage,
                    ProfileImageUrl = editVM.ProfileImageUrl,
                    City = editVM.City,
                    State = editVM.State
                };


                _dashboardRepository.Update(updatedUser);
                return RedirectToAction("Index");
            }

            return View("EditUserProfile", editVM);
        }
    }
}
