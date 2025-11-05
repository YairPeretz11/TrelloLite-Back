using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrelloClone.Api.Models;

namespace TrelloClone.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ListController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ListController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List>> GetList(int id)
        {
            try
            {
                var list = await _context.Lists
                    .Include(l => l.Tasks)
                    .FirstOrDefaultAsync(l => l.ListId == id);

                if (list == null)
                {
                    return NotFound();
                }

                return list;
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("board/{boardId}")]
        public async Task<IActionResult> GetListsByBoard(int boardId)
        {
            try
            {
                var lists = await _context.Lists
                    .Where(l => l.BoardId == boardId)
                    .Include(l => l.Tasks)
                    .ToListAsync();

                return Ok(lists);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateList([FromBody] List list)
        {
            try
            {
                _context.Lists.Add(list);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetList), new { id = list.ListId }, list);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteList(int id)
        {
            try
            {
                var list = await _context.Lists.FindAsync(id);
                if (list == null)
                {
                    return NotFound();
                }

                _context.Lists.Remove(list);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

}