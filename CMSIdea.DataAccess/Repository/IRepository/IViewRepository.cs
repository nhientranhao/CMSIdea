using CMSIdea.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSIdea.DataAccess.Repository.IRepository
{
    public interface IViewRepository : IRepository<View>
    {
        void Update(View view);

    }
}
