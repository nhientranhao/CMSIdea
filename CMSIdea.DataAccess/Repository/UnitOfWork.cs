using CMSIdea.DataAccess.Data;
using CMSIdea.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSIdea.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ApplicationDbContext _db;

        public ICategoryRepository Category { get; private set; }


        public ITopicRepository Topics { get; private set; }

        public IDepartmentRepository Department { get; private set; }

        public IIdeaRepository Idea { get; private set; }

        public ICommentRepository Comment { get; private set; }

        public IReactRepository React { get; private set; }

        public IViewRepository View { get; private set; }


        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;

            Category = new CategoryRepository(_db);
            Department = new DepartmentRepository(_db);
            Topics = new TopicRepository(_db);
            Idea = new IdeaRepository(_db);

        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
