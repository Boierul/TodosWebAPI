using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Mvc;
using TodosWebAPI.Models;

namespace TodosWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodosController : ControllerBase
    {
        private ITodoService TodoService;

        // Dependency injection
        public TodosController(ITodoService todoService)
        {
            TodoService = todoService;
        }

        [HttpGet]
        public async Task<ActionResult<IList<Todo>>>
            GetTodos([FromQuery] int? userId, [FromQuery] bool? isCompleted)
        {
            try
            {
                // Access ToDo service
                IList<Todo> todos = await TodoService.GetTodosAsync();

                // Filter the list by isCompleted parameter
                if (isCompleted != null)
                {
                    return Ok(todos.Where(t => t.IsCompleted == isCompleted));
                }

                // Filter the list by userID parameter
                if (userId != null)
                {
                    return Ok(todos.Where(t => t.UserId == userId));
                }
                return Ok(todos);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Todo>> AddTodo([FromBody] Todo todo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Todo addTodo = await TodoService.AddTodoAsync(todo);
                return Created($"/{addTodo.TodoId}", addTodo); // return newly added to-do, to get the auto generated id
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult> DeleteTodo([FromRoute] int id)
        {
            try
            {
                await TodoService.RemoveTodoAsync(id);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpPatch]
        [Route("{id:int}")]
        public async Task<ActionResult<Todo>> UpdateTodo([FromBody] Todo todo)
        {
            try
            {
                Todo updatedTodo = await TodoService.UpdateAsync(todo);
                return Ok(updatedTodo);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
    }
}