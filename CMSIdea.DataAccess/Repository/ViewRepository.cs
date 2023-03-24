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
    public class ViewRepository : Repository<View>, IViewRepository
    {

        private readonly ApplicationDbContext _db;


        public ViewRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;

        }


        public void Update(View view)
        {
            _db.Views.Update(view);
        }
    }
}
