using System.ComponentModel.DataAnnotations;

namespace ProjectAPI.Models
{
    public class ProjectTask
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        public DateTime? DueDate { get; set; }

        public bool IsCompleted { get; set; }

        public Guid ProjectId { get; set; }
        public Project Project { get; set; } = null!;
    }
}