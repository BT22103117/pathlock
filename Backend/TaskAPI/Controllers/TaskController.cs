using Microsoft.AspNetCore.Mvc;
using TaskAPI.Models;
using TaskAPI.Services;

namespace TaskAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Task>> GetAllTasks()
        {
            return Ok(_taskService.GetAllTasks());
        }

        [HttpGet("{id}")]
        public ActionResult<Task> GetTaskById(Guid id)
        {
            try
            {
                return Ok(_taskService.GetTaskById(id));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public ActionResult<Task> CreateTask(Task task)
        {
            var createdTask = _taskService.CreateTask(task);
            return CreatedAtAction(nameof(GetTaskById), new { id = createdTask.Id }, createdTask);
        }

        [HttpPut("{id}")]
        public ActionResult<Task> UpdateTask(Guid id, Task task)
        {
            if (id != task.Id)
                return BadRequest();

            try
            {
                return Ok(_taskService.UpdateTask(task));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteTask(Guid id)
        {
            try
            {
                _taskService.DeleteTask(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}