using ToDoApi.Dtos.toDo;
using ToDoApi.Models;

namespace ToDoApi.Interfaces
{
    public interface IToDoRepository
    {
        Task<List<ToDo>> GetAllAsync(int page, int pageSize,string? sort, string? order, string? searchTerm);
        Task<ToDo> GetByIdAsync(int id);
        Task<ToDo> UpdateAsync(int id, ToDoDto todo);
        Task<ToDo> CreateAsync(ToDoDto todo);
        Task<ToDo> Delete(int id);

    }
}
