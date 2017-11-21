using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WebApi.DataAccess;
using WebApi.Model;

namespace WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Produces("application/json")]
    [Route("api/todo")]
    public class TodoController : Controller
    {
        private readonly ILogger _logger;
        private readonly TodoRepository todoRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        public TodoController(IConfiguration configuration, ILogger<TodoController> logger)
        {
            todoRepository = new TodoRepository(configuration);
            _logger = logger;
        }
        
        /// <summary>
        /// List All TodoItens
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<TodoItem>), 200)]
        [ProducesResponseType(typeof(List<TodoItem>), 400)]
        public IEnumerable<TodoItem> GetAll()
        {
            _logger.LogInformation("GetAll was requested - init");
            return todoRepository.FindAll();

        }

        /// <summary>
        /// Find TodoItem by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetTodo")]
        public IActionResult GetById(int id)
        {
            var item =  todoRepository.FindByID(id);
            
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        /// <summary>
        /// Creates a TodoItem
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     POST /Todo
        ///     {
        ///        "id": 1,
        ///        "name": "Item1",
        ///        "isComplete": true
        ///     }
        /// </remarks>
        /// <param name="item"></param>
        /// <returns>A newly-created TodoItem</returns>
        /// <response code="201">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response> 
        [HttpPost]
        [ProducesResponseType(typeof(TodoItem), 201)]
        [ProducesResponseType(typeof(TodoItem), 400)]
        public IActionResult Create([FromBody] TodoItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            todoRepository.Add(item);
         
            return CreatedAtRoute("GetTodo", new { id = item.Id }, item);
        }

        /// <summary>
        /// Update TodoItem
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] TodoItem item)
        {
            if (item == null || item.Id != id)
            {
                return BadRequest();
            }

            var todo = todoRepository.FindByID(id);

            if (todo == null)
                return NotFound();
            
            todo.IsComplete = item.IsComplete;
            todo.Name = item.Name;

            todoRepository.Update(todo);
            
            return new NoContentResult();
        }

        /// <summary>
        /// Delete TodoItem
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            todoRepository.Remove(id);
            return new NoContentResult();
        }
    }
}