using System.ComponentModel.DataAnnotations;

namespace LMS_Assig_.Models
{
    public class AssignSection
    {
        public int id { get; set; }
        [Required]
        public string Teacher { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int SectionId { get; set; }

    }
}
