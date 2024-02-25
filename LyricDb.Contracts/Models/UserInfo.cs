using System;

namespace LyricDb.Contracts.Models;

public class UserInfo
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public UserRoleEnum Role { get; set; } = UserRoleEnum.None;
    public int ContributionPoint { get; set; } = 0;
}

public enum UserRoleEnum
{
    None,
    User,
    Reviewer,
    Admin
}