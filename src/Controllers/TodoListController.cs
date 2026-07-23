using Microsoft.AspNetCore.Mvc;
using TodoList.Models;
using System.Text.Json;

namespace TodoList.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoListsController : ControllerBase
    {
        private readonly string filePath = "TodoList.txt";

        private List<TodoList.Models.TodoList> LoadTodos()
        {
            if (!System.IO.File.Exists(filePath))
            {
                return new List<TodoList.Models.TodoList>();
            }

            string json = System.IO.File.ReadAllText(filePath);

            return JsonSerializer.Deserialize<List<TodoList.Models.TodoList>>(json)
                ?? new List<TodoList.Models.TodoList>();
        }

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

        [HttpGet]
        public IActionResult GetAll()
        {
            var todos = LoadTodos();

            return Ok(todos);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var todos = LoadTodos();

            var todo = todos.FirstOrDefault(t => t.Id == id);

            if(todo == null)
                return NotFound();
            return Ok(todo);
        }

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