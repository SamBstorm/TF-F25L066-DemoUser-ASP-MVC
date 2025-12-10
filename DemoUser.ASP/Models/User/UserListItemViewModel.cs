namespace DemoUser.ASP.Models.User;

public class UserListItemViewModel
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
}