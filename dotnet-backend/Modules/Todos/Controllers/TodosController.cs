using dotnet_backend.Data;
using dotnet_backend.Modules.Todos.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace dotnet_backend.Modules.Todos.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TodosController(AppDbContext context) : ControllerBase
    {

        // GET: api/v1/todos
        [HttpGet]
        public async Task<IActionResult> GetTodos()
        {
            var todos = await context.Todos.OrderByDescending(t => t.CreatedAt).ToListAsync();

            return Ok(new { success = true, data = todos });
        }

        // GET: api/v1/todos/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodo(Guid id)
        {
            var todo = await context.Todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound(new { success = false, message = "Todo not found" });
            }

            return Ok(new { success = true, data = todo });
        }

        // POST api/v1/todos
        [HttpPost]
        public async Task<IActionResult> CreateTodo([FromBody] TodoItem newTodo)
        {
            context.Todos.Add(newTodo);
            await context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetTodo),
                new { id = newTodo.Id },
                new { success = true, data = newTodo }
            );
        }

        // PUT api/v1/todos/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(Guid id, [FromBody] TodoItem updatedTodo)
        {
            var todo = await context.Todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound(new { success = false, message = "Todo not found" });
            }

            todo.Title = updatedTodo.Title;
            todo.Description = updatedTodo.Description;
            todo.Priority = updatedTodo.Priority;
            todo.DueDate = updatedTodo.DueDate;
            todo.UpdatedAt = DateTime.UtcNow;

            await context.SaveChangesAsync();

            return Ok(new { success = true, data = todo });
        }

        // PATCH api/v1/todos/{id}/status
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] string status)
        {
            var todo = await context.Todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound(new { success = false, message = "Todo not found" });
            }

            todo.Status = status;
            todo.UpdatedAt = DateTime.UtcNow;

            await context.SaveChangesAsync();

            return Ok(new { success = true, data = todo });
        }

        // DELETE api/v1/todos/{id} - Soft Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(Guid id)
        {
            var todo = await context.Todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound(new { success = false, message = "Todo not found" });
            }

            todo.DeletedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();

            return Ok(new { success = true, message = "Todo deleted successfully" });
        }

        // PUT api/v1/todos/{id}/restore - Restore
        [HttpPut("{id}/restore")]
        public async Task<IActionResult> RestoreTodo(Guid id)
        {
            var todo = await context.Todos.IgnoreQueryFilters().FirstOrDefaultAsync(t => t.Id == id);

            if (todo == null || todo.DeletedAt == null)
            {
                return NotFound(new { success = false, message = "Soft Deleted Todo not found" });
            }

            todo.DeletedAt = null;
            todo.UpdatedAt = DateTime.UtcNow;

            await context.SaveChangesAsync();

            return Ok(new { success = true, data = todo });
        }

        // DELETE api/v1/todos/trash - Clear Soft Deleted
        [HttpDelete("trash")]
        public async Task<IActionResult> EmptyTrash()
        {
            var trashedTodos = await context.Todos
                .IgnoreQueryFilters()
                .Where(t => t.DeletedAt != null)
                .ToListAsync();

            if (trashedTodos.Count == 0)
            {
                return Ok(new { success = true, message = "Trash is already empty." });
            }

            context.Todos.RemoveRange(trashedTodos);

            await context.SaveChangesAsync();

            return Ok(new { success = true, message = $"Permanently deleted {trashedTodos.Count} todos" });
        }

        // DELETE api/v1/todos/all
        [HttpDelete("all")]
        public async Task<IActionResult> DeleteAllTodos()
        {
            var allTodos = await context.Todos.IgnoreQueryFilters().ToListAsync();

            if (allTodos.Count == 0)
            {
                return Ok(new { success = true, message = "The repository is already completely empty" });
            }

            context.Todos.RemoveRange(allTodos);

            return Ok(new { success = true, message = $"Permanently deleted {allTodos.Count} todos. The repository is now completelty empty." });
        }
    }
}
