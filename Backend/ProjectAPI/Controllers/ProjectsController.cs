using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ProjectAPI.DTOs;
using ProjectAPI.Services;

namespace ProjectAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        private Guid GetUserId()
        {
            return Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        }

        [HttpGet]
        public async Task<ActionResult<List<ProjectDto>>> GetUserProjects()
        {
            try
            {
                var projects = await _projectService.GetUserProjects(GetUserId());
                return Ok(projects);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> GetProject(Guid id)
        {
            try
            {
                var project = await _projectService.GetProject(id, GetUserId());
                return Ok(project);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ProjectDto>> CreateProject(CreateProjectDto createProjectDto)
        {
            try
            {
                var project = await _projectService.CreateProject(createProjectDto, GetUserId());
                return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProjectDto>> UpdateProject(Guid id, UpdateProjectDto updateProjectDto)
        {
            try
            {
                var project = await _projectService.UpdateProject(id, updateProjectDto, GetUserId());
                return Ok(project);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProject(Guid id)
        {
            try
            {
                await _projectService.DeleteProject(id, GetUserId());
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}