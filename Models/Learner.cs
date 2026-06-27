using System.ComponentModel.DataAnnotations;

namespace LearnerOpsLms.Models
{
    public class Learner
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Full name")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Department { get; set; } = string.Empty;

        [Display(Name = "Enrolled")]
        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;
    }
}