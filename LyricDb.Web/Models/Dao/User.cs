using System.ComponentModel.DataAnnotations;
using LyricDb.Web.Models.Constants;
using Microsoft.AspNetCore.Identity;

namespace LyricDb.Web.Models.Dao;

public class User : IdentityUser<Guid>
{
    public required string Avatar { get; set; }
    public UserRole Role { get; set; } = UserRole.None;
    public int ContributionPoint { get; set; } = 0;
}

public enum UserRole
{
    None,
    User,
    Reviewer,
    Admin
}

public static class UserRoleMapper
{
    public static string Map(UserRole role)
    {
        return role switch
        {
            UserRole.User => AuthRole.User,
            UserRole.Reviewer => AuthRole.Reviewer,
            UserRole.Admin => AuthRole.Admin,
            _ => AuthRole.None
        };
    }
}