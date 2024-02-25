namespace LyricDb.Web.Models.Constants;

public class AuthRole
{
    public const string None = $"role_none,{User}";
    public const string User = $"role_user,{Reviewer}";
    public const string Reviewer = $"role_reviewer,{Admin}";
    public const string Admin = $"role_admin";
}