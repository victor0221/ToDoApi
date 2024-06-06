using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ToDoApi.Dtos.loginRegister;
using ToDoApi.Dtos.toDo;
using ToDoApi.Interfaces;
using ToDoApi.Models;

namespace ToDoApi.Controllers
{
    [ApiController]
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/toDo")]
    [Authorize(Roles = "Admin, User")]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoRepository _repo;
        public ToDoController(IToDoRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("")]
        [SwaggerOperation("ToDo get all")]
        [SwaggerResponse(200, "OK", typeof(IEnumerable<ToDo>))]
        public async Task<IActionResult> ToDoGetAll([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] string? sort, [FromQuery] string? order, [FromQuery] string? searchTerm)
        {
            try
            {
                var toDo = await _repo.GetAllAsync(page, pageSize, sort, order, searchTerm);
                return Ok(toDo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id:int}")]
        [SwaggerOperation("ToDo get one")]
        [SwaggerResponse(200, "OK", typeof(IEnumerable<ToDo>))]
        [SwaggerResponse(204, "No Response", typeof(IEnumerable<string>))]
        public async Task<IActionResult> ToDoGetOne([FromRoute] int id)
        {
            try
            {
                var toDo = await _repo.GetByIdAsync(id);
                return Ok(toDo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id:int}")]
        [SwaggerOperation("ToDo update one")]
        [SwaggerResponse(200, "OK", typeof(IEnumerable<ToDo>))]
        public async Task<IActionResult> ToDoUpdateOne([FromRoute] int id, [FromBody] ToDoDto toDoDto)
        {
            try
            {
                var toDos = await _repo.UpdateAsync(id, toDoDto);
                return Ok(toDos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id:int}")]
        [SwaggerOperation("ToDo delete one")]
        [SwaggerResponse(200, "OK", typeof(IEnumerable<ToDo>))]
        public async Task<IActionResult> ToDoDeleteOne([FromRoute] int id)
        {
            try
            {
                var toDos = await  _repo.Delete(id);
                return Ok(toDos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("")]
        [SwaggerOperation("ToDo create one")]
        [SwaggerResponse(200, "OK", typeof(IEnumerable<ToDo>))]
        public async Task<IActionResult> ToDoCreateOne([FromBody] ToDoDto ToDoCreatDto)
        {
            try
            {
                var toDos = await _repo.CreateAsync(ToDoCreatDto);
                return Ok(toDos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
