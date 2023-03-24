using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CMSIdea.Models.ViewModel
{
    public class RoleUserVM
    {
        [Required]
        [Display(Name = "User")]
        public string UserId { get; set; }
        [Required]

        [Display(Name = "Role")]
        public string RoleId { get; set; }
    }
}
