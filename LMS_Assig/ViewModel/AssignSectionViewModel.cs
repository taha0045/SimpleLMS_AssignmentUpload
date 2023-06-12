using LMS_Assig_.Models;

namespace LMS_Assig_.ViewModel
{
    public class AssignSectionViewModel
    {

        public List<SectionDetails> SectionSelectList { get; set; }
        public List<ApplicationUser> Teachers { get; set; }


        public AssignSection AssignSection { get; set; }

    }
}
