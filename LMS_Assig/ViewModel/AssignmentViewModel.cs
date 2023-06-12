using LMS_Assig_.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LMS_Assig_.ViewModel
{
    public class AssignmentViewModel
    {
       

      
        public Assignment Assignment { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public List<SectionDetails> SectionSelectList { get; set; }
        public AssignSection AssignSection { get; set; }

        public List<Assignment> assignments { get; set; }
        public List<Assignment> FilterAssignments { get; set; }


    }
}
