namespace LyricDb.Web.Models.Dto.Requests;

public class UserLoginRequest
{
    public required string Account { get; set; }
    public required string Password { get; set; }
}