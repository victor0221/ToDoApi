using System.ComponentModel.DataAnnotations;

namespace ToDoApi.Dtos.toDo
{
    public class ToDoDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime? completeBy { get; set; }
        public bool? completed { get; set; }
    }
}
