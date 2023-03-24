using CMSIdea.DataAccess.Data;
using CMSIdea.DataAccess.Repository.IRepository;
using CMSIdea.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSIdea.DataAccess.Repository
{
    public class IdeaRepository : Repository<Idea>, IIdeaRepository
    {
        private ApplicationDbContext _db;

        public IdeaRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public void Update(Idea obj)
        {
            var objFromDb = _db.Ideas.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Title = obj.Title;
                objFromDb.Brief = obj.Brief;
                objFromDb.Content = obj.Content;
                objFromDb.FilePath = obj.FilePath;
                objFromDb.DateTime = obj.DateTime;
                objFromDb.UserId = obj.UserId;
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.TopicId = obj.TopicId;
                if (obj.FilePath != null)
                {
                    objFromDb.FilePath = obj.FilePath;
                }
            }
        }
    }
}

