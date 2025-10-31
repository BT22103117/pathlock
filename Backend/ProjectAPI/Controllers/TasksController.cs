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
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        private Guid GetUserId()
        {
            return Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        }

        [HttpGet("project/{projectId}")]
        public async Task<ActionResult<List<ProjectTaskDto>>> GetProjectTasks(Guid projectId)
        {
            try
            {
                var tasks = await _taskService.GetProjectTasks(projectId, GetUserId());
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectTaskDto>> GetTask(Guid id)
        {
            try
            {
                var task = await _taskService.GetTask(id, GetUserId());
                return Ok(task);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("project/{projectId}")]
        public async Task<ActionResult<ProjectTaskDto>> CreateTask(Guid projectId, CreateTaskDto createTaskDto)
        {
            try
            {
                var task = await _taskService.CreateTask(projectId, createTaskDto, GetUserId());
                return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProjectTaskDto>> UpdateTask(Guid id, UpdateTaskDto updateTaskDto)
        {
            try
            {
                var task = await _taskService.UpdateTask(id, updateTaskDto, GetUserId());
                return Ok(task);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTask(Guid id)
        {
            try
            {
                await _taskService.DeleteTask(id, GetUserId());
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}