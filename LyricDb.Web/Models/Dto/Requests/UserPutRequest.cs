using LyricDb.Web.Models.Constants;
using LyricDb.Web.Models.Dao;

namespace LyricDb.Web.Models.Dto.Requests;

public class UserPutRequest
{
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public string? OldPassword { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
    public UserRole? Role { get; set; }
}