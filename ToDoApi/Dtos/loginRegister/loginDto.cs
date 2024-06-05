using System.ComponentModel.DataAnnotations;

namespace ToDoApi.Dtos.loginRegister
{
    public class loginDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
