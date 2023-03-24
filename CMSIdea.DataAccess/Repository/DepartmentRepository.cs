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
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {

        private readonly ApplicationDbContext _db;

        public DepartmentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;

        }


        public void Update(Department department)
        {
            _db.Departments.Update(department);
        }
    }
}