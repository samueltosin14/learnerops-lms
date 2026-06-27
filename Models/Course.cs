using System.ComponentModel.DataAnnotations;

namespace LearnerOpsLms.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Course title")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Level { get; set; } = string.Empty;

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Created")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}