using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrelloClone.Api.Models;

namespace TrelloClone.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class TaskItemController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TaskItemController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetTaskItem(int id)
        {
            try
            {
                var taskItem = await _context.TaskItems.FindAsync(id);

                if (taskItem == null)
                {
                    return NotFound();
                }

                return taskItem;
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("list/{listId}")]
        public async Task<IActionResult> GetTaskItemsByList(int listId)
        {
            try
            {
                var taskItems = await _context.TaskItems
                    .Where(t => t.ListId == listId)
                    .ToListAsync();

                return Ok(taskItems);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("board/{boardId}")]
        public async Task<IActionResult> GetTaskItemsByBoard(int boardId)
        {
            try
            {
                var taskItems = await _context.TaskItems
                    .Include(t => t.List)
                    .Where(t => t.List.BoardId == boardId)
                    .ToListAsync();

                return Ok(taskItems);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTaskItem([FromBody] TaskItem taskItem)
        {
            try
            {
                _context.TaskItems.Add(taskItem);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetTaskItem), new { id = taskItem.TaskItemId }, taskItem);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskItem(int id)
        {
            var taskItem = await _context.TaskItems.FindAsync(id);
            if (taskItem == null)
            {
                return NotFound();
            }

            _context.TaskItems.Remove(taskItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTaskItem(int id, [FromBody] TaskItem updatedTaskItem)
        {
            if (id != updatedTaskItem.TaskItemId)
            {
                return BadRequest(new { message = "TaskItem ID mismatch" });
            }

            _context.Entry(updatedTaskItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Concurrency error occurred" });
            }
        }
    }
}