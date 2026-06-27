using LearnerOpsLms.Models;
using Microsoft.EntityFrameworkCore;

namespace LearnerOpsLms.Data
{
    public class LearnerOpsDbContext : DbContext
    {
        public LearnerOpsDbContext(DbContextOptions<LearnerOpsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Learner> Learners { get; set; }

        public DbSet<Assessment> Assessments { get; set; }

        public DbSet<EvidenceItem> EvidenceItems { get; set; }
    }
}