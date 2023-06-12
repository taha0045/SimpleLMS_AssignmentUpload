using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Collections.Specialized.BitVector32;

namespace LMS_Assig_.Models
{
    public class Assignment
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

      
        public string FilePath { get; set; }

        [Required]
        public int SectionID { get; set; }

        public string SectionName { get; set; }

        [Required]
        public DateTime UploadDate { get; set; }


        //public string AnsFilePath { get; set; }

        public Status Status { get; set; }
        public int assgnID { get; set; }

        public string userId { get; set; } 



        //[StringLength(255)]
        //public string Content { get; internal set; }

        //public string AssignmentFile { get; set; }



    }

}
