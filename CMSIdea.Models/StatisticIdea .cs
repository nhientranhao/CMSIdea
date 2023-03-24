using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSIdea.Models
{
    public class StatisticIdea : Idea
    {
        public int Like { get; set; }

        public int Dislike { get; set; }

        public int CountView { get; set; }

        public int CountComment { get; set; }

        public int CountIdea { get; set; }

    }
}
