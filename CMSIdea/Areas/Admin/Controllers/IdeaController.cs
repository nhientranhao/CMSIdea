using CMSIdea.DataAccess.Data;
using CMSIdea.DataAccess.Repository.IRepository;
using CMSIdea.Models;
using CMSIdea.Models.ViewModel;
using CMSIdea.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Bcpg;

namespace CMSIdea.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class IdeaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;
        public IdeaController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, ApplicationDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
        }

        //================================ 1. VIEW INDEX  =====================================//
        public IActionResult Index()
        {
            var objIdeaList = _context.Ideas.Include(i => i.Category).Include(i => i.Topic).ToList();
            return View(objIdeaList);

        }

        //================================ END  VIEW INDEX  =====================================//


        //================================ 2. UPSERT  ==========================================//
        public IActionResult Upsert(int? id)
        {
            IdeaVM ideaVM = new IdeaVM();
            ideaVM.idea = new Idea();

            ideaVM.idea.UserId = User.Identity.Name;

            ideaVM.CategoryList = _unitOfWork.Category.GetAll().Select(
                u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }
                );
            ideaVM.TopicList = _unitOfWork.Topics.GetAll().Select(
                u => new SelectListItem { Text = u.Name, Value = u.Id.ToString() }
                );

            if (id == null || id == 0)
            {

                return View(ideaVM);
            }
            else
            {
                ideaVM.idea = _unitOfWork.Idea.GetFirstOrDefault(u => u.Id == id);

            }


            return View(ideaVM);
        }

        // -------UPSERT POST---------//
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(IdeaVM obj, IFormFile? file)
        {

            obj.CategoryList = _unitOfWork.Category.GetAll().Select(
                u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }
                );
            obj.TopicList = _unitOfWork.Topics.GetAll().Select(
                u => new SelectListItem { Text = u.Name, Value = u.Id.ToString() });

            if (ModelState.IsValid)
            {

                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"file\idea");
                    var extension = Path.GetExtension(file.FileName);
                    if (obj.idea.FilePath != null)
                    {
                        var oideaRFilePath = Path.Combine(wwwRootPath, obj.idea.FilePath.TrimStart('\\'));
                        if (System.IO.File.Exists(oideaRFilePath))
                        {
                            System.IO.File.Delete(oideaRFilePath);
                        }
                    }
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.idea.FilePath = @"file\idea\" + fileName + extension;

                }
                if (obj.idea.Id == 0)
                {

                    _unitOfWork.Idea.Add(obj.idea);
                    TempData["Success"] = "Create idea successful";
                }
                else
                {
                    _unitOfWork.Idea.Update(obj.idea);
                    TempData["Success"] = "Edit idea successful";
                }



                _emailSender.SendEmailAsync("windsunny23@gmail.com", "Hello Idea", "<p>wel</p>");
                _unitOfWork.Save();
                return Redirect("/Admin/Topic/ViewIdea/" + obj.idea.TopicId);

            }
            return View(obj);
        }

        //================================ 2 END UPSERT =====================================//



        //================================ 3. DELETE  =====================================//
        public IActionResult Delete(int? id)
        {

            IdeaVM ideaVM = new IdeaVM();
            ideaVM.idea = new Idea();
            ideaVM.idea.UserId = User.Identity.Name;

            ideaVM.CategoryList = _unitOfWork.Category.GetAll().Select(
                u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }
                );
            ideaVM.TopicList = _unitOfWork.Topics.GetAll().Select(
                u => new SelectListItem { Text = u.Name, Value = u.Id.ToString() }
                );


            if (id == null || id == 0)
            {
                return View(ideaVM);
            }
            else
            {
                ideaVM.idea = _unitOfWork.Idea.GetFirstOrDefault(u => u.Id == id);

            }

            return View(ideaVM);

        }

        //-------------Delete Post----------------//

        [HttpPost, ActionName("Delete")]
        //TOKEN avoid fake method
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _unitOfWork.Idea.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            else
            {
                _unitOfWork.Idea.Remove(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Delete idea successful";
                return RedirectToAction("Index");
            }

        }

        //================================  END DELETE =====================================//



        //================================ 4. LIKE =====================================//
        public IActionResult Like(int? id, string type = null)
        {
            int IdeaId = (int)id;

            Idea idea = _unitOfWork.Idea.GetFirstOrDefault(u => u.Id == IdeaId);

            React react = new React();
            react.Count = 1;

            react.UserId = User.Identity.Name;
            react.IdeaId = IdeaId;
            var objReact = _context.Reacts.Include(r => r.Idea);
            List<React> Reacts = _context.Reacts.Where(a => a.IdeaId == IdeaId && a.UserId == User.Identity.Name).ToList();

            int topicid = idea.TopicId;
            Topic topic = _unitOfWork.Topics.GetFirstOrDefault(u => u.Id == topicid);
            if (DateTime.Now <= topic.FinalCloseDate)
            {
                if (Reacts.Count > 0)
                {
                    Reacts[0].Count = 1;
                    _context.SaveChanges();
                }
                else
                {
                    _context.Add(react);
                    _context.SaveChanges();
                }

            }

            if (type == "2")
            {
                return Redirect("/Admin/Idea/ViewDetail/" + idea.TopicId);
            }
            if (type == "3")
            {
                return Redirect("/Admin/Idea/ViewDetail/" + idea.Id);

            }

            return Redirect("/Admin/Topic/ViewIdea/" + idea.TopicId);

        }

        //================================ END LIKE =====================================//


        //================================ 5. DISLIKE =====================================//
        public IActionResult DisLike(int? id, string type = null)
        {
            int IdeaId = (int)id;
            Idea idea = _unitOfWork.Idea.GetFirstOrDefault(u => u.Id == IdeaId);

            React react = new React();
            react.Count = -1;

            react.UserId = User.Identity.Name;
            react.IdeaId = IdeaId;

            var objReact = _context.Reacts.Include(r => r.Idea);
            List<React> Reacts = _context.Reacts.Where(a => a.IdeaId == IdeaId && a.UserId == User.Identity.Name).ToList();

            int topicid = idea.TopicId;
            Topic topic = _unitOfWork.Topics.GetFirstOrDefault(u => u.Id == topicid);
            if (DateTime.Now <= topic.FinalCloseDate)
            {
                if (Reacts.Count > 0)
                {
                    Reacts[0].Count = -1;
                    _context.SaveChanges();
                }
                else
                {
                    _context.Add(react);
                    _context.SaveChanges();
                }

            }


            //
            if (type == "2")
            {
                return Redirect("/Admin/Idea/ViewDetail/" + idea.TopicId);
            }
            //
            if (type == "3")
            {
                return Redirect("/Admin/Idea/ViewDetail/" + idea.Id);

            }
            return Redirect("/Admin/Topic/ViewIdea/" + idea.TopicId);
        }

        //================================ END DISLIKE =====================================//



        //================================ 6. VIEW DETAIL =====================================//

        public IActionResult ViewDetail(int? id)
        {
            int IdeaId = (int)id;

            Idea idea = _unitOfWork.Idea.GetFirstOrDefault(u => u.Id == id);

            idea.Category = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == idea.CategoryId);
            idea.Topic = _unitOfWork.Topics.GetFirstOrDefault(u => u.Id == idea.TopicId);

            ViewBag.comments = _context.Comments.Where(i => i.IdeaId == IdeaId).ToList();

            //finalclosedate close comment
            Topic topic = _context.Topics.SingleOrDefault(a => a.Id == idea.TopicId);
            if (DateTime.Now >= topic.FinalCloseDate)
            {
                ViewBag.finalCloseDate = "1";

            }

            CMSIdea.Models.View view = new View();
            view.IdeaId = (int)id;

            view.UserId = User.Identity.Name;

            view.ViewTime = 1;
            List<View> dsView = _context.Views.Where(a => a.IdeaId == IdeaId && a.UserId == User.Identity.Name).ToList();
            if (dsView.Count == 0)
            {
                _context.Views.Add(view);
                _context.SaveChanges();
            }

            IdeaReacts ideaR = new IdeaReacts();
            ideaR.Id = idea.Id;
            ideaR.Title = idea.Title;
            ideaR.Brief = idea.Brief;
            ideaR.Content = idea.Content;
            ideaR.DateTime = idea.DateTime;
            ideaR.UserId = idea.UserId;
            ideaR.Category = idea.Category;
            ideaR.CategoryId = idea.CategoryId;
            ideaR.TopicId = idea.TopicId;
            ideaR.Topic = idea.Topic;
            ideaR.FilePath = idea.FilePath;

            int countLike = _context.Reacts.Where(i => i.IdeaId == idea.Id && i.Count == 1).Count();
            int countDisLike = _context.Reacts.Where(i => i.IdeaId == idea.Id && i.Count == -1).Count();
            ideaR.Like = countLike;
            ideaR.Dislike = countDisLike;

            int countView = _context.Views.Where(i => i.IdeaId == idea.Id).Count();
            ideaR.CountView = countView;

            return View(ideaR);

        }


        //================================ END VIEW DETAIL =====================================//

        //================================ 7. COMMENT =====================================//
        public IActionResult Comment(int? id, string? comment)
        {
            int IdeaId = (int)id;

            Idea idea = _unitOfWork.Idea.GetFirstOrDefault(u => u.Id == IdeaId);

            int topicid = idea.TopicId;
            Topic topic = _unitOfWork.Topics.GetFirstOrDefault(u => u.Id == topicid);
            if (DateTime.Now <= topic.FinalCloseDate)
            {

                Comment objectComment = new Comment();
                objectComment.IdeaId = IdeaId;
                objectComment.Text = comment;
                objectComment.UserId = User.Identity.Name;
                objectComment.DateTime = DateTime.Now;
                _emailSender.SendEmailAsync("windsunny23@gmail.com", "Comment", "<p>ur idea</p>");
                _context.Comments.Add(objectComment);
                _context.SaveChanges();

            }

            return Redirect("/Admin/Idea/ViewDetail/" + id);
        }
        //================================ END COMMENT =====================================//

    }
}
