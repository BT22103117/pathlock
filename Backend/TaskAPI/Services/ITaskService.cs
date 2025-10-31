using TaskAPI.Models;

namespace TaskAPI.Services
{
    public interface ITaskService
    {
        IEnumerable<Task> GetAllTasks();
        Task GetTaskById(Guid id);
        Task CreateTask(Task task);
        Task UpdateTask(Task task);
        void DeleteTask(Guid id);
    }
}