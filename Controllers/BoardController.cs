using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrelloClone.Api.Models;

namespace TrelloClone.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BoardController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [HttpGet]
        public async Task<IActionResult> GetBoards()
        {
            try
            {
                var boards = await _context.Boards
                    .Include(b => b.Lists)
                    .ThenInclude(l => l.Tasks)
                    .ToListAsync();
                return Ok(boards);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBoard([FromBody] Board board)
        {
            try
            {
                _context.Boards.Add(board);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetBoards), new { id = board.BoardId }, board);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBoard(int id)
        {
            var board = await _context.Boards.FindAsync(id);
            if (board == null)
            {
                return NotFound();
            }

            _context.Boards.Remove(board);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }

}
