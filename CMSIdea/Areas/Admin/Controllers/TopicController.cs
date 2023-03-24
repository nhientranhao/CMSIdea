using CMSIdea.DataAccess.Data;
using CMSIdea.DataAccess.Repository.IRepository;
using CMSIdea.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CMSIdea.Areas.Admin.Controllers
{

    [Area("Admin")]

    public class TopicController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public TopicController(IUnitOfWork unitOfWork, ApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }


        //================================ 1.VIEW INDEX  =====================================//
        public IActionResult Index()
        {

            IEnumerable<Topic> objTopicList = _unitOfWork.Topics.GetAll();
            return View(objTopicList);
        }

        //================================ END  =====================================//

        //================================ 2. CREATE  =====================================//
        public IActionResult Create()
        {
            return View();
        }

        //------------------------Create Post---------------------------

        [HttpPost]
        //TOKEN avoid fake method
        [ValidateAntiForgeryToken]
        public IActionResult Create(Topic obj)
        {
            // Validate Require
            if (ModelState.IsValid)
            {
                _unitOfWork.Topics.Add(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Create Topic successful";
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

            var topicFromDb = _unitOfWork.Topics.GetFirstOrDefault(u => u.Id == id);
            if (topicFromDb == null)
            {

                return NotFound();
            }
            return View(topicFromDb);
        }

        //----------Edit Post---------------

        [HttpPost]
        //TOKEN avoid fake method
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Topic obj)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Topics.Update(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Update Topic successful";
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

            var topicFromDb = _unitOfWork.Topics.GetFirstOrDefault(u => u.Id == id);
            if (topicFromDb == null)
            {

                return NotFound();
            }
            return View(topicFromDb);
        }

        //-----------------Delete Post--------------

        [HttpPost, ActionName("Delete")]
        //TOKEN avoid fake method
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _unitOfWork.Topics.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            else
            {
                _unitOfWork.Topics.Remove(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Delete Topic successful";
                return RedirectToAction("Index");
            }

        }

        //================================  END DELETE =====================================//



        //================================ 3. VIEW IDEA  =====================================//
        public IActionResult ViewIdea(int? id)
        {
            List<IdeaReacts> ideaRs = new List<IdeaReacts>();


            int topicid = (int)id;
            var objIdeaList = _context.Ideas.Where(i => i.TopicId == topicid).Include(i => i.Category).Include(i => i.Topic).ToList();
            foreach (var item in objIdeaList)
            {

                IdeaReacts ideaR = new IdeaReacts();
                ideaR.Id = item.Id;
                ideaR.Title = item.Title;
                ideaR.Brief = item.Brief;
                ideaR.Content = item.Content;
                ideaR.DateTime = item.DateTime;
                ideaR.UserId = item.UserId;
                ideaR.Category = item.Category;
                ideaR.CategoryId = item.CategoryId;
                ideaR.TopicId = item.TopicId;
                ideaR.Topic = item.Topic;
                ideaR.FilePath = item.FilePath;

                int countLike = _context.Reacts.Where(i => i.IdeaId == item.Id && i.Count == 1).Count();
                int countDisLike = _context.Reacts.Where(i => i.IdeaId == item.Id && i.Count == -1).Count();
                ideaR.Like = countLike;
                ideaR.Dislike = countDisLike;

                int countView = _context.Views.Where(i => i.IdeaId == item.Id).Count();
                ideaR.CountView = countView;

                ideaRs.Add(ideaR);
            }

            IEnumerable<IdeaReacts> end = ideaRs;

            Topic topic = _context.Topics.SingleOrDefault(a => a.Id == topicid);

            if (DateTime.Now >= topic.CloseDate)
            {
                ViewBag.closeDate = "1";
            }

            return View(end);

        }
        //================================  END DELETE =====================================//
    }
}
