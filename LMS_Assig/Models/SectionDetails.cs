using System.ComponentModel.DataAnnotations;

namespace LMS_Assig_.Models
{
    public class SectionDetails
    {
        private static readonly IEnumerable<object> Section;

        [Key]
        public int SectionId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
    }
}
