using CMSIdea.DataAccess.Data;
using CMSIdea.DataAccess.Repository.IRepository;
using CMSIdea.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CMSIdea.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _userManager;
        ApplicationDbContext _db;
        public UserController(Microsoft.AspNetCore.Identity.UserManager<IdentityUser> userManager, ApplicationDbContext db, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _db = db;
            _unitOfWork = unitOfWork;
        }


        public IActionResult Index()
        {
            var dd = _userManager.GetUserId(HttpContext.User);
            return View(_db.ApplicationUsers.ToList());
        }
        //   [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            ViewData["DepartmentId"] = new SelectList(_unitOfWork.Department.GetAll(), "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ApplicationUser user)
        {

            ViewData["DepartmentId"] = new SelectList(_unitOfWork.Department.GetAll(), "Id", "Name");

            if (ModelState.IsValid)
            {

                var result = await _userManager.CreateAsync(user, user.PasswordHash);

                if (result.Succeeded)
                {

                    
                    TempData["Success"] = "Create user successful";
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }


            return View();
        }


        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUser user)
        {
            var userInfo = _db.ApplicationUsers.FirstOrDefault(c => c.Id == user.Id);
            if (userInfo == null)
            {
                return NotFound();
            }
            userInfo.Fullname = user.Fullname;
            userInfo.Email = user.Email;
            userInfo.PhoneNumber = user.PhoneNumber;

            var result = await _userManager.UpdateAsync(userInfo);
            if (result.Succeeded)
            {
                TempData["Success"] = "Edit user successful";
                return RedirectToAction(nameof(Index));
            }
            return View(userInfo);
        }


        public async Task<IActionResult> Delete(string id)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ApplicationUser user)
        {
            var userInfo = _db.ApplicationUsers.FirstOrDefault(c => c.Id == user.Id);
            if (userInfo == null)
            {
                return NotFound();

            }
            _db.ApplicationUsers.Remove(userInfo);
            int rowAffected = _db.SaveChanges();
            if (rowAffected > 0)
            {
                TempData["Success"] = "Delete user successful";
                return RedirectToAction(nameof(Index));
            }
            return View(userInfo);
        }
    }
}
