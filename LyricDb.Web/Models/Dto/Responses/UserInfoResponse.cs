using LyricDb.Web.Interfaces;
using LyricDb.Web.Models.Constants;
using LyricDb.Web.Models.Dao;
using Riok.Mapperly.Abstractions;

namespace LyricDb.Web.Models.Dto.Responses;

public class UserInfoResponse
{
    public Guid Id { get; set; }
    public required string UserName { get; set; }
    public required string Avatar { get; set; }
    public UserRole Role { get; set; }
}

[Mapper]
public partial class UserInfoResponseMapper : IMapper<User, UserInfoResponse>
{
    public partial UserInfoResponse Map(User from);
}