namespace PersonsApp.Dtos.Users
{
    public class UserEditDto : UserCreateDto
    {
        public bool ChangePassword { get; set; }
    }
}