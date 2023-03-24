using CMSIdea.DataAccess.Repository.IRepository;
using CMSIdea.Models;
using Microsoft.AspNetCore.Mvc;

namespace CMSIdea.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(IUnitOfWork unitOfWork)
        {
            //1 and 2 _db = db;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Department> objDepartmentList = _unitOfWork.Department.GetAll();
            return View(objDepartmentList);
        }

        //================================ 1. CREATE  =====================================//
        public IActionResult Create()
        {

            return View();
        }

        //--------------------------------Create Post--------------------------------

        [HttpPost]
        //TOKEN avoid fake method
        [ValidateAntiForgeryToken]
        public IActionResult Create(Department obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Department.Add(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Create Department successful";
                return RedirectToAction("Index");
            }
            return View(obj);

        }

        //================================  END CREATE  =====================================//


        //================================ 2. EDIT  =====================================//
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var DepartmentFromDb = _unitOfWork.Department.GetFirstOrDefault(u => u.Id == id);
            if (DepartmentFromDb == null)
            {

                return NotFound();
            }
            return View(DepartmentFromDb);
        }

        //--------------------------------Edit Post--------------------------------

        [HttpPost]
        //TOKEN avoid fake method
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Department obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Department.Update(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Update Department successful";

                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //================================  END EDIT =====================================//


        //================================ 3. DELETE  =====================================//
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var DepartmentFromDb = _unitOfWork.Department.GetFirstOrDefault(u => u.Id == id);
            if (DepartmentFromDb == null)
            {

                return NotFound();
            }
            return View(DepartmentFromDb);
        }

        //--------------------------------Delete Post--------------------------------

        [HttpPost, ActionName("Delete")]
        //TOKEN avoid fake method
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _unitOfWork.Department.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {

                return NotFound();
            }
            else
            {

                _unitOfWork.Department.Remove(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Delete Department successful";
                return RedirectToAction("Index");
            }

        }

        //================================  END DELETE =====================================//
    }
}
