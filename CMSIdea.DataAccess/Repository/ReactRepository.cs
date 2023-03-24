using CMSIdea.DataAccess.Data;
using CMSIdea.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSIdea.DataAccess.Repository.IRepository
{
    public class ReactRepository : Repository<React>, IReactRepository
    {

        private readonly ApplicationDbContext _db;

        public ReactRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;

        }

        public void Update(React react)
        {
            _db.Reacts.Update(react);
        }
    }
}