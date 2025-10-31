using Microsoft.EntityFrameworkCore;
using ProjectAPI.Models;
using ProjectAPI.DTOs;

namespace ProjectAPI.Services
{
    public interface IProjectService
    {
        Task<List<ProjectDto>> GetUserProjects(Guid userId);
        Task<ProjectDto> GetProject(Guid projectId, Guid userId);
        Task<ProjectDto> CreateProject(CreateProjectDto createProjectDto, Guid userId);
        Task<ProjectDto> UpdateProject(Guid projectId, UpdateProjectDto updateProjectDto, Guid userId);
        Task DeleteProject(Guid projectId, Guid userId);
    }

    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _context;

        public ProjectService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProjectDto>> GetUserProjects(Guid userId)
        {
            var projects = await _context.Projects
                .Include(p => p.Tasks)
                .Where(p => p.UserId == userId)
                .ToListAsync();

            return projects.Select(p => new ProjectDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                CreatedAt = p.CreatedAt,
                Tasks = p.Tasks.Select(t => new ProjectTaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    DueDate = t.DueDate,
                    IsCompleted = t.IsCompleted,
                    ProjectId = t.ProjectId
                }).ToList()
            }).ToList();
        }

        public async Task<ProjectDto> GetProject(Guid projectId, Guid userId)
        {
            var project = await _context.Projects
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == projectId && p.UserId == userId);

            if (project == null)
                throw new Exception("Project not found.");

            return new ProjectDto
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                CreatedAt = project.CreatedAt,
                Tasks = project.Tasks.Select(t => new ProjectTaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    DueDate = t.DueDate,
                    IsCompleted = t.IsCompleted,
                    ProjectId = t.ProjectId
                }).ToList()
            };
        }

        public async Task<ProjectDto> CreateProject(CreateProjectDto createProjectDto, Guid userId)
        {
            var project = new Project
            {
                Title = createProjectDto.Title,
                Description = createProjectDto.Description,
                CreatedAt = DateTime.UtcNow,
                UserId = userId
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return new ProjectDto
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                CreatedAt = project.CreatedAt,
                Tasks = new List<ProjectTaskDto>()
            };
        }

        public async Task<ProjectDto> UpdateProject(Guid projectId, UpdateProjectDto updateProjectDto, Guid userId)
        {
            var project = await _context.Projects
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == projectId && p.UserId == userId);

            if (project == null)
                throw new Exception("Project not found.");

            project.Title = updateProjectDto.Title;
            project.Description = updateProjectDto.Description;

            await _context.SaveChangesAsync();

            return new ProjectDto
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                CreatedAt = project.CreatedAt,
                Tasks = project.Tasks.Select(t => new ProjectTaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    DueDate = t.DueDate,
                    IsCompleted = t.IsCompleted,
                    ProjectId = t.ProjectId
                }).ToList()
            };
        }

        public async Task DeleteProject(Guid projectId, Guid userId)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == projectId && p.UserId == userId);

            if (project == null)
                throw new Exception("Project not found.");

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }
    }
}