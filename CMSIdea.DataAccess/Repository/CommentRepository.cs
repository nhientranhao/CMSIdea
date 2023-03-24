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
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {

        private readonly ApplicationDbContext _db;

        public CommentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;

        }


        public void Update(Comment comment)
        {
            _db.Comments.Update(comment);
        }
    }
}