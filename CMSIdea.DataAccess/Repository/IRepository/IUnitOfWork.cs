using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSIdea.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {

        ICategoryRepository Category { get; }

        ITopicRepository Topics { get; }

        IDepartmentRepository Department { get; }


        IIdeaRepository Idea { get; }

        ICommentRepository Comment { get; }

        IReactRepository React { get; }

        IViewRepository View { get; }

        void Save();

    }
}
