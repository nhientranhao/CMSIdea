using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSIdea.Models
{

    public class Idea
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Brief { get; set; }

        [Required]
        public string Content { get; set; }

        [ValidateNever]
        public string? FilePath { get; set; }

        [Required]
        public DateTime DateTime { get; set; }= DateTime.Now;

        public string UserId { get; set; }
        /*[ValidateNever]
        [ForeignKey("UserId")]

        public ApplicationUser User { get; set; }*/

        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [ValidateNever]
        [ForeignKey("CategoryId")]

        public Category Category { get; set; }

        [Display(Name = "Topic")]
        public int TopicId { get; set; }
        [ValidateNever]
        [ForeignKey("TopicId")]
        public Topic Topic { get; set; }



    }
}
