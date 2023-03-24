using CMSIdea.DataAccess.Repository.IRepository;
using CMSIdea.Models;
using Microsoft.AspNetCore.Mvc;

namespace CMSIdea.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll();
            return View(objCategoryList);
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
        public IActionResult Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Create category successful";
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

            var categoryFromDb = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
            if (categoryFromDb == null)
            {

                return NotFound();
            }
            return View(categoryFromDb);
        }

        //--------------------------------Edit Post--------------------------------

        [HttpPost]
        //TOKEN avoid fake method
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Update category successful";
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
            var categoryFromDb = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
            if (categoryFromDb == null)
            {

                return NotFound();
            }
            return View(categoryFromDb);
        }

        //--------------------------------Delete Post--------------------------------

        [HttpPost, ActionName("Delete")]
        //TOKEN avoid fake method
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {

                return NotFound();
            }
            else
            {
                _unitOfWork.Category.Remove(obj);
                _unitOfWork.Save();

                //show ms
                TempData["Success"] = "Delete category successful";
                // return View();
                return RedirectToAction("Index");
            }

        }

        //================================  END DELETE =====================================//
    }
}
