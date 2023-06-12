using LMS_Assig_.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace LMS_Assig_.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
      

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<SectionDetails> SectionDetails { get; set; }
        public DbSet<Assignment>? Assignment { get; set; }
        public DbSet<AssignSection>? AssignSection { get; set; }
    }
}
