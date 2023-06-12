using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS_Assig_.Models
{
    public class ApplicationUser : IdentityUser
    {
        
        public string FirstName { get; set; }
       
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public int Phone { get; set; }

        public string Role { get; set; }
        public int? SectionId { get; set; }

        [ForeignKey("SectionId")]
        [ValidateNever]
        public SectionDetails SectionDetails { get; set; }


        [NotMapped]
        public string FullName { get
            {
                return FirstName + " " + LastName;
            }
        }



    }
}
