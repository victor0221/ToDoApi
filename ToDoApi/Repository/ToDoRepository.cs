using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Data;
using ToDoApi.Dtos.toDo;
using ToDoApi.Interfaces;
using ToDoApi.Models;

namespace ToDoApi.Repository
{
    public class ToDoRepository : IToDoRepository
    {
        private readonly ToDoContext _context;
        public ToDoRepository(ToDoContext context)
        {
                _context = context;
        }
        public async Task<ToDo> CreateAsync(ToDoDto todo)
        {
            if (todo == null)
            {
                throw new Exception("ToDo data is null");
            }

            var ToDo = new ToDo
            {
                Name = todo.Name,
                completeBy = todo.completeBy,
                completed = todo.completed,
            };

            await _context.ToDo.AddAsync(ToDo);
            await _context.SaveChangesAsync();
            return ToDo;

        }

        public async Task<ToDo> Delete(int id)
        {
            await _context.ToDo.Where(t=> t.Id == id).ExecuteDeleteAsync();
            var item = await GetByIdAsync(id);
            return item;
        }

        public async Task<List<ToDo>> GetAllAsync(int page, int pageSize, string? sort, string? order, string? searchTerm)
        {
            page = page == 0 ? 1 : page;
            pageSize = pageSize == 0 ? 25 : pageSize;

            var query = _context.ToDo.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(t => t.Name.Contains(searchTerm));
            }

            if (!string.IsNullOrWhiteSpace(sort))
            {
                switch (sort.ToLower())
                {
                    case "name":
                        query = order?.ToLower() == "desc" ? query.OrderByDescending(t => t.Name) : query.OrderBy(t => t.Name);
                        break;
                    default:
                        query = order?.ToLower() == "desc" ? query.OrderByDescending(t => t.Id) : query.OrderBy(t => t.Id);
                        break;
                }
            }

            var startIndex = (page - 1) * pageSize;
            var pagedToDos = await query.Skip(startIndex).Take(pageSize).ToListAsync();

            return pagedToDos;
        }

        public async Task<ToDo> GetByIdAsync(int id)
        {
            var toDo = await _context.ToDo.FirstOrDefaultAsync(t => t.Id == id);
            if (toDo == null) new Exception("no user with that id");
            return toDo;
        }

        public async Task<ToDo> UpdateAsync(int id, ToDoDto todo)
        {
            var toDo = await GetByIdAsync(id);
            if (toDo == null) throw new Exception("No ToDo by that id");

            todo.Name = todo.Name;
            todo.completed = todo.completed;
            todo.completeBy = todo.completeBy;

            _context.SaveChanges();
            return toDo;

        }
    }
}
