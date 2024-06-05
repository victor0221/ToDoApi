namespace ToDoApi.Dtos.loginRegister
{
    public class newUserDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public IList<string> Roles { get; set; }
    }
}
