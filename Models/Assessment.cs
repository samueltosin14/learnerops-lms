using System.ComponentModel.DataAnnotations;

namespace LearnerOpsLms.Models
{
    public class Assessment : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Assessment title")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Range(1, 10000)]
        [Display(Name = "Maximum score")]
        public int MaxScore { get; set; }

        [Range(1, 10000)]
        [Display(Name = "Pass score")]
        public int PassScore { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Created")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            if (PassScore > MaxScore)
            {
                yield return new ValidationResult(
                    "The pass score cannot be greater than the maximum score.",
                    new[] { nameof(PassScore) });
            }
        }
    }
}