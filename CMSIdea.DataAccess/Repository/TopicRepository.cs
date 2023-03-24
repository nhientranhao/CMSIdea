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
    public class TopicRepository : Repository<Topic>, ITopicRepository
    {

        private readonly ApplicationDbContext _db;


        public TopicRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;

        }


        public void Update(Topic topic)
        {
            _db.Topics.Update(topic);
        }
    }
}
