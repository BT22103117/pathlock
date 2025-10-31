using System.ComponentModel.DataAnnotations;

namespace ProjectAPI.DTOs
{
    public class ProjectTaskDto
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        public DateTime? DueDate { get; set; }

        public bool IsCompleted { get; set; }

        public Guid ProjectId { get; set; }
    }

    public class CreateTaskDto
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        public DateTime? DueDate { get; set; }
    }

    public class UpdateTaskDto
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        public DateTime? DueDate { get; set; }

        public bool IsCompleted { get; set; }
    }
}