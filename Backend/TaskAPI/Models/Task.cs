namespace TaskAPI.Models
{
    public class Task
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}