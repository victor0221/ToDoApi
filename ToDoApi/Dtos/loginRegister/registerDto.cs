using System.ComponentModel.DataAnnotations;

namespace ToDoApi.Dtos.loginRegister
{
    public class registerDto
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        public string? Phonenumber { get; set; }
        public string? Password { get; set; }
    }
}
