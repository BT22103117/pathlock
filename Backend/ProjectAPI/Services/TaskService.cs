using Microsoft.EntityFrameworkCore;
using ProjectAPI.Models;
using ProjectAPI.DTOs;

namespace ProjectAPI.Services
{
    public interface ITaskService
    {
        Task<List<ProjectTaskDto>> GetProjectTasks(Guid projectId, Guid userId);
        Task<ProjectTaskDto> GetTask(Guid taskId, Guid userId);
        Task<ProjectTaskDto> CreateTask(Guid projectId, CreateTaskDto createTaskDto, Guid userId);
        Task<ProjectTaskDto> UpdateTask(Guid taskId, UpdateTaskDto updateTaskDto, Guid userId);
        Task DeleteTask(Guid taskId, Guid userId);
    }

    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _context;

        public TaskService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProjectTaskDto>> GetProjectTasks(Guid projectId, Guid userId)
        {
            var tasks = await _context.Tasks
                .Include(t => t.Project)
                .Where(t => t.ProjectId == projectId && t.Project.UserId == userId)
                .ToListAsync();

            return tasks.Select(t => new ProjectTaskDto
            {
                Id = t.Id,
                Title = t.Title,
                DueDate = t.DueDate,
                IsCompleted = t.IsCompleted,
                ProjectId = t.ProjectId
            }).ToList();
        }

        public async Task<ProjectTaskDto> GetTask(Guid taskId, Guid userId)
        {
            var task = await _context.Tasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == taskId && t.Project.UserId == userId);

            if (task == null)
                throw new Exception("Task not found.");

            return new ProjectTaskDto
            {
                Id = task.Id,
                Title = task.Title,
                DueDate = task.DueDate,
                IsCompleted = task.IsCompleted,
                ProjectId = task.ProjectId
            };
        }

        public async Task<ProjectTaskDto> CreateTask(Guid projectId, CreateTaskDto createTaskDto, Guid userId)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == projectId && p.UserId == userId);

            if (project == null)
                throw new Exception("Project not found.");

            var task = new ProjectTask
            {
                Title = createTaskDto.Title,
                DueDate = createTaskDto.DueDate,
                ProjectId = projectId
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return new ProjectTaskDto
            {
                Id = task.Id,
                Title = task.Title,
                DueDate = task.DueDate,
                IsCompleted = task.IsCompleted,
                ProjectId = task.ProjectId
            };
        }

        public async Task<ProjectTaskDto> UpdateTask(Guid taskId, UpdateTaskDto updateTaskDto, Guid userId)
        {
            var task = await _context.Tasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == taskId && t.Project.UserId == userId);

            if (task == null)
                throw new Exception("Task not found.");

            task.Title = updateTaskDto.Title;
            task.DueDate = updateTaskDto.DueDate;
            task.IsCompleted = updateTaskDto.IsCompleted;

            await _context.SaveChangesAsync();

            return new ProjectTaskDto
            {
                Id = task.Id,
                Title = task.Title,
                DueDate = task.DueDate,
                IsCompleted = task.IsCompleted,
                ProjectId = task.ProjectId
            };
        }

        public async Task DeleteTask(Guid taskId, Guid userId)
        {
            var task = await _context.Tasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == taskId && t.Project.UserId == userId);

            if (task == null)
                throw new Exception("Task not found.");

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }
}