using System.ComponentModel.DataAnnotations;

namespace LearnerOpsLms.Models
{
    public class EvidenceItem
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Evidence title")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Notes { get; set; } = string.Empty;

        [Required]
        [StringLength(250)]
        [Display(Name = "File path")]
        public string FilePath { get; set; } = string.Empty;

        [Display(Name = "Archived")]
        public bool IsArchived { get; set; } = false;

        [Display(Name = "Uploaded")]
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}