using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSIdea.Models
{
    public class React
    {
        [Key]
        public int Id { get; set; }

        public int Count { get; set; }
        /*
         React: Integer
        *Hint:
        1: like
        -1:Dislike
        0: no react
         */
        public string UserId { get; set; }
      /*  [ValidateNever]
        [ForeignKey("UserId")]*/

        //public ApplicationUser User { get; set; }

        public int IdeaId { get; set; }
        [ValidateNever]
        [ForeignKey("IdeaId")]

        public Idea Idea { get; set; }
    }
}
