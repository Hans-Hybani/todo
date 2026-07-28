using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TodoList.Models;
using System.Text.Json;

namespace TodoList.Controllers
{
    // REFACTOR the old TodosController.cs controller

    // ==================================================

    // Changes and reasons:

    //  1. JSON serialization:
    //     The old controller built the JSON manually by concatenating strings (“{\”i\“: ” + s[0] + ...) .
    //     A quotation mark or comma in the text of a todo would result in invalid JSON.
    //     Hence the use of System.Text.Json (Deserialize/Serialize), which automatically handles the escaping of special characters.
    //     See:
    //            return JsonSerializer.Deserialize<List<TodoList.Models.TodoList>>(json)
    //            ?? new List<TodoList.Models.TodoList>();

    //  2. Storage format:
    //     Data is stored in a fully named JSON file called TodoList.txt, read and written via JsonSerializer.
    //     This works reliably with any text content.

    //  3. ToTitleCase:
    //     The recursive title-casing function (ToTitleCase) has been removed from the server.
    //     Why? Angular natively provides the `titlecase` pipe,
    //     which is applied directly in the template ({{ todo.title | titlecase }}).
    //     ================================================
    //     The client now handles the display, so the server doesn’t have to reimplement logic already provided natively by the framework.
    //     ================================================

    //  4. Added features:
    //     CRUD
    //     Create / Update / Delete have been added, with input validation using Data Annotations on the model
    //     Two-level input validation:
    //       - client-side (Angular): Validators.maxLength(2000), Validators.required
    //       - server-side (C#, see Models/TodoList.cs): [StringLength(2000, ...)]
    //     Automatically verified by ASP.NET using the [ApiController] attribute.

    //  5. Robustness:
    //     Explicit handling of the case where the storage file does not yet exist or is empty (LoadTodos),
    //     if (string.IsNullOrWhiteSpace(json))
    //         {
    //            return new List<TodoList.Models.TodoList>();
    //         }
    //
    //     This is something the old code did not check before reading the file.

    [ApiController]
    [Route("api/[controller]")]
    public class TodoListController : ControllerBase
    {
        private readonly string filePath = "TodoList.txt";
        private List<TodoList.Models.TodoList> LoadTodos()
        {
            if (!System.IO.File.Exists(filePath))
            {
                return new List<TodoList.Models.TodoList>();
            }
            string json = System.IO.File.ReadAllText(filePath);

            //  Case where the storage file does not yet exist or is empty
                if (string.IsNullOrWhiteSpace(json))
                {
                    return new List<TodoList.Models.TodoList>();
                }
            return JsonSerializer.Deserialize<List<TodoList.Models.TodoList>>(json)
                ?? new List<TodoList.Models.TodoList>();
        }

        // Overwrites the entire storage file with the provided list; a simple approach suitable for a text file
        private void SaveTodos(List<TodoList.Models.TodoList> todos)
        {
            string json = JsonSerializer.Serialize(
                todos,
                new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                });

            System.IO.File.WriteAllText(filePath, json);
        }

        // GET api/TodoList — returns the complete list of to-dos.
        [HttpGet]
        public IActionResult GetAll()
        {
            var todos = LoadTodos();

            return Ok(todos);
        }

        // GET api/TodoList/{id} — returns a specific todo, or a 404 if it doesn't exist.
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var todos = LoadTodos();
            var todo = todos.FirstOrDefault(t => t.Id == id);

            if(todo == null)
                return NotFound();
            return Ok(todo);
        }

        // POST api/TodoList — creates a new todo.
        // Validation (required title, maximum lengths) is defined using Data Annotations.
        [HttpPost]
        public IActionResult Create(TodoList.Models.TodoList todo)
        {
            var todos = LoadTodos();

            todo.Id = todos.Count == 0 
                ? 1 
                : todos.Max(t => t.Id) + 1;
            todos.Add(todo);

            SaveTodos(todos);

            return Ok(todo);
        }

        // PUT api/TodoList/{id} — updates an existing todo (title, due date, notes).
        // Returns a 404 if the id does not match any todo.
        [HttpPut("{id}")]
        public IActionResult Update(int id, TodoList.Models.TodoList todo)
        {
            var todos = LoadTodos();
            var existing = todos.FirstOrDefault(t => t.Id == id);

            if(existing == null)
                return NotFound();

            existing.Title = todo.Title;
            existing.IsDone = todo.IsDone;
            existing.DueDate = todo.DueDate;
            existing.Notes = todo.Notes;

            SaveTodos(todos);

            return Ok(existing);
        }

        // DELETE api/TodoList/{id} — deletes a todo. Returns a 404 if the id does not match any existing todo.
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var todos = LoadTodos();
            var todo = todos.FirstOrDefault(t => t.Id == id);

            if(todo == null)
                return NotFound();
            todos.Remove(todo);

            SaveTodos(todos);

            return Ok();
        }
    }
}