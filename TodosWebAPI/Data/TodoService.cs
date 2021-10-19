using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TodosWebAPI.Models;

namespace Data
{
    public class TodoService : ITodoService
    {
        private string todoFile = "todos.json";
        private IList<Todo> todos;

        public TodoService()
        {
            if (!File.Exists(todoFile))
            {
                Seed();
                WriteTodosToFile();
            }
            else
            {
                string content = File.ReadAllText(todoFile);
                todos = JsonSerializer.Deserialize<List<Todo>>(content);
            }
        }

        public async Task<IList<Todo>> GetTodosAsync()
        {
            List<Todo> temp = new List<Todo>(todos);
            return temp;
        }

        public async Task<Todo> AddTodoAsync(Todo todo)
        {
            int max = todos.Max(todo => todo.TodoId);
            todo.TodoId = (++max);
            todos.Add(todo);
            WriteTodosToFile();
            return todo;
        }
        

        public async Task RemoveTodoAsync(int todoId)
        {
            Todo toRemove = todos.First(t => t.TodoId == todoId);
            todos.Remove(toRemove);
            WriteTodosToFile();
        }

        public async Task<Todo> UpdateAsync(Todo todo)
        {
            Todo toUpdate = todos.First(t => t.TodoId == todo.TodoId);
            if (toUpdate == null)
            {
                throw new Exception($"Did not find todo with id: {todo.TodoId}");
            }
            toUpdate.IsCompleted = todo.IsCompleted;
            WriteTodosToFile();
            return toUpdate;
        }

        // public Todo Get(int id)
        // {
        //     return todos.FirstOrDefault(t => t.TodoId == id);
        // }

        private void WriteTodosToFile()
        {
            string todosAsJson = JsonSerializer.Serialize(todos);
            File.WriteAllText(todoFile, todosAsJson);
        }

        private void Seed()
        {
            Todo[] ts =
            {
                new Todo {UserId = 1, TodoId = 1, Title = "Do dishes", IsCompleted = false},
                new Todo {UserId = 1, TodoId = 2, Title = "Walk the dog", IsCompleted = false},
                new Todo {UserId = 2, TodoId = 3, Title = "Do DNP homework", IsCompleted = true},
                new Todo {UserId = 3, TodoId = 4, Title = "Eat breakfast", IsCompleted = false},
                new Todo {UserId = 4, TodoId = 5, Title = "Mow lawn", IsCompleted = true},
            };
            todos = ts.ToList();
        }
    }
}