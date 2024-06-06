using System.ComponentModel.DataAnnotations;

namespace ToDoApi.Models
{
    public class ToDo
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime? completeBy { get; set; }
        public bool? completed { get; set; }
        public DateTime? createdAt { get; set; } = DateTime.Now;
    }
}
