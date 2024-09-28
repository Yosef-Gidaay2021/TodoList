
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListAPI.Models;
using TodoListAPI.Data;
using System.Threading.Tasks;
using System.Linq;

namespace TodoListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoItemsController : ControllerBase
    {
        private readonly TodoContext _context;

        public ToDoItemsController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/ToDoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoItem>>> GetIncompleteToDoItems()
        {
            // Fetch all ToDoItems where CompletedDate is null
            var incompleteToDoItems = await _context.ToDoItems
                .Where(t => t.CompletedDate == null)
                .ToListAsync();

            return Ok(incompleteToDoItems);
        }

        // Get a specific ToDoItem by Id
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoItem>> GetToDoItemById(long id)
        {
            var toDoItem = await _context.ToDoItems.FindAsync(id);

            if (toDoItem == null)
            {
                return NotFound();
            }

            return Ok(toDoItem);
        }

        // Create a new ToDoItem
        [HttpPost]
        public async Task<ActionResult<ToDoItem>> CreateToDoItem(ToDoItem toDoItem)
        {
            _context.ToDoItems.Add(toDoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetToDoItemById), new { id = toDoItem.Id }, toDoItem);
        }

        // Update a ToDoItem and set the CompletedDate to current datetime
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateToDoItem(long id, ToDoItem toDoItem)
        {
            if (id != toDoItem.Id)
            {
                return BadRequest();
            }

            var itemToUpdate = await _context.ToDoItems.FindAsync(id);
            if (itemToUpdate == null)
            {
                return NotFound();
            }

            itemToUpdate.CompletedDate = DateTime.UtcNow;

            _context.Entry(itemToUpdate).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

         // Delete a specific ToDoItem by Id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoItem(long id)
        {
            var toDoItem = await _context.ToDoItems.FindAsync(id);
            if (toDoItem == null)
            {
                return NotFound();
            }

            _context.ToDoItems.Remove(toDoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
