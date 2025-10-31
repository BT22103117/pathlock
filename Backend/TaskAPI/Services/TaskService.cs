using TaskAPI.Models;

namespace TaskAPI.Services
{
    public class TaskService : ITaskService
    {
        private readonly List<Task> _tasks;

        public TaskService()
        {
            _tasks = new List<Task>();
        }

        public IEnumerable<Task> GetAllTasks()
        {
            return _tasks;
        }

        public Task GetTaskById(Guid id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
                throw new KeyNotFoundException("Task not found");
            return task;
        }

        public Task CreateTask(Task task)
        {
            task.Id = Guid.NewGuid();
            task.CreatedAt = DateTime.UtcNow;
            _tasks.Add(task);
            return task;
        }

        public Task UpdateTask(Task task)
        {
            var existingTask = _tasks.FirstOrDefault(t => t.Id == task.Id);
            if (existingTask == null)
                throw new KeyNotFoundException("Task not found");

            existingTask.Description = task.Description;
            existingTask.IsCompleted = task.IsCompleted;

            return existingTask;
        }

        public void DeleteTask(Guid id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
                throw new KeyNotFoundException("Task not found");

            _tasks.Remove(task);
        }
    }
}