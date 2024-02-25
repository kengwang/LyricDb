using LyricDb.Web.Interfaces;
using LyricDb.Web.Models.Dao;
using Riok.Mapperly.Abstractions;

namespace LyricDb.Web.Models.Dto.Responses;

public class LyricInfoResponse
{
    public Guid Id { get; set; }
    public required DateTime CreateTime { get; set; }
    public required UserInfoResponse Submitter { get; set; }
    public required SongInfoResponse Song { get; set; }
    public UserInfoResponse? Reviewer { get; set; }
    public bool Approved { get; set; }
}

[Mapper]
public partial class LyricToLyricInfoMapper : IMapper<Lyric, LyricInfoResponse>
{
    public partial LyricInfoResponse Map(Lyric from);
}