using CMSIdea.DataAccess.Data;
using CMSIdea.DataAccess.Repository.IRepository;
using CMSIdea.Models;
using CMSIdea.Models.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO.Compression;
using System.Text;

namespace CMSIdea.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class StatisticController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _webHostEnvironment;

        //create constructor
        public StatisticController(IUnitOfWork unitOfWork, ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            //1 and 2 _db = db;
            _unitOfWork = unitOfWork;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {

            IEnumerable<Topic> objTopicList = _unitOfWork.Topics.GetAll();
            //   var objTopicList= _db.Categories.ToList();
            return View(objTopicList);
        }
        //-----------Idea

        public IActionResult StatisticIdea()
        {
            var objIdeaList = _context.Ideas.Include(i => i.Category).Include(i => i.Topic).ToList();
            List<IdeaReacts> ideaRList = new List<IdeaReacts>();
            foreach (var item in objIdeaList)
            {
                ideaRList.Add(new IdeaReacts()
                {
                    Brief = item.Brief,
                    CategoryId = item.CategoryId,
                    Title = item.Title,
                    Content = item.Content,
                    DateTime = item.DateTime,
                    UserId = item.UserId,
                    TopicId = item.TopicId,
                    FilePath = item.FilePath,
                    Category = item.Category,
                    Topic = item.Topic,

                    CountView = _context.Views.Where(a => a.IdeaId == item.Id).Count(),
                    Like = _context.Reacts.Where(a => a.IdeaId == item.Id && a.Count == 1).Count(),
                    Dislike = _context.Reacts.Where(a => a.IdeaId == item.Id && a.Count == -1).Count(),
                    CountComment = _context.Comments.Where(a => a.IdeaId == item.Id).Count()
                });
            }

            IEnumerable<IdeaReacts> ideaRs = ideaRList;
            return View(ideaRs);
        }

        //----------------------Topic
        public IActionResult StatisticTopic()
        {
            IEnumerable<Topic> objTopicList = _unitOfWork.Topics.GetAll();
            List<StatisticIdea> staIdeaList = new List<StatisticIdea>();

            foreach (Topic topic in objTopicList)
            {
                List<Idea> ideas = _context.Ideas.Where(a => a.TopicId == topic.Id).ToList();
                int Like = 0;
                int Dislike = 0;
                int CountView = 0;
                int CountComment = 0;
                int CountIdea = _context.Ideas.Where(a => a.TopicId == topic.Id).Count();

                foreach (Idea item in ideas)
                {
                    Like += _context.Reacts.Where(a => a.IdeaId == item.Id && a.Count == 1).Count();
                    Dislike += _context.Reacts.Where(a => a.IdeaId == item.Id && a.Count == -1).Count();
                    CountView += _context.Views.Where(a => a.IdeaId == item.Id).Count();
                    CountComment += _context.Comments.Where(a => a.IdeaId == item.Id).Count();
                }
                staIdeaList.Add(new StatisticIdea()
                {
                    Title = topic.Name,
                    Id = topic.Id,
                    Like = Like,
                    Dislike = Dislike,
                    CountView = CountView,
                    CountComment = CountComment,
                    CountIdea = CountIdea
                });
            }

            IEnumerable<StatisticIdea> staIdeas = staIdeaList;
            return View(staIdeas);
        }

        //--------
        public IActionResult ExportExcel(int? id)
        {
            var builder = new StringBuilder();

            builder.AppendLine("Title,name,DateTime,UserId,CategoryId, Content, Filepath, Total Like, Total Dislike, Total View");

            IEnumerable<Idea> ideas = _context.Ideas.Where(a => a.TopicId == id);
            List<IdeaReacts> ideaExelList = new List<IdeaReacts>();

            foreach (var item in ideas)
            {

                ideaExelList.Add(new IdeaReacts()
                {
                    Brief = item.Brief,
                    CategoryId = item.CategoryId,
                    Title = item.Title,
                    Content = item.Content,
                    DateTime = item.DateTime,
                    UserId = item.UserId,
                    TopicId = item.TopicId,
                    FilePath = item.FilePath,
                    Category = item.Category,
                    Topic = item.Topic,

                    CountView = _context.Views.Where(a => a.IdeaId == item.Id).Count(),
                    Like = _context.Reacts.Where(a => a.IdeaId == item.Id && a.Count == 1).Count(),
                    Dislike = _context.Reacts.Where(a => a.IdeaId == item.Id && a.Count == -1).Count(),
                    CountComment = _context.Comments.Where(a => a.IdeaId == item.Id).Count()

                });
            }

            IEnumerable<IdeaReacts> ideaExels = ideaExelList;


            foreach (var item in ideaExels)
            {


                builder.AppendLine($"{item.Title},{item.Brief},{item.DateTime}, {item.UserId},{item.CategoryId}, {item.Content}, {item.FilePath},{item.Like},{item.Dislike},{item.CountView}");

            }

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "Idea" + id + "idea.csv");

        }


        //-----
        public IActionResult StatisticSD()
        {
            return View(nameof(StatisticSD));
        }
        public IActionResult ExportZip(int? id)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            List<string> files = new List<string>();

            IEnumerable<Idea> ideas = _context.Ideas.Where(a => a.TopicId == id);
            int c = ideas.Count();


            foreach (var idea in ideas)
            {
                if (System.IO.File.Exists(wwwRootPath + "//" + idea.FilePath))
                {
                    files.Add(wwwRootPath + "//" + idea.FilePath);
                }

            }

            var tempFile = Path.GetTempFileName();

            using (var zipFile = System.IO.File.Create(tempFile))
            using (var zipArchive = new ZipArchive(zipFile, ZipArchiveMode.Create))
            {
                foreach (var file in files)
                {
                    zipArchive.CreateEntryFromFile(file, Path.GetFileName(file));
                }
            }

            var stream = new FileStream(tempFile, FileMode.Open);
            return File(stream, "application/zip", "Topic" + id + "idea.zip");

        }

    }
}
