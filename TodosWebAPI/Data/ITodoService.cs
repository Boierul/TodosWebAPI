using System.Collections.Generic;
using System.Threading.Tasks;
using TodosWebAPI.Models;

namespace Data
{
    public interface ITodoService
    {
        Task<IList<Todo>> GetTodosAsync();
        Task<Todo> AddTodoAsync(Todo todo);
        Task RemoveTodoAsync(int todoId);
        Task<Todo> UpdateAsync(Todo todo);
    }
}