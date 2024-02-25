namespace LyricDb.Web.Models.Dto.Requests;

public class UserRegisterRequest
{
    public required string Name { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
}