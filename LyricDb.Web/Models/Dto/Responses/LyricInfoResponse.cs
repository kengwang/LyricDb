using LyricDb.Contracts.Models;
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
    public required int Duration { get; set; }
    public UserInfoResponse? Reviewer { get; set; }
    public LyricStatus Status { get; set; }
    public string? Author { get; set; }
    public string? Translator { get; set; }
    public string? Transliterator { get; set; }
    public string? Timeline { get; set; }
    public string? Proofreader { get; set; }
}

[Mapper]
public partial class LyricToLyricInfoMapper : IMapper<Lyric, LyricInfoResponse>
{
    public partial LyricInfoResponse Map(Lyric from);
}