using LyricDb.Web.Interfaces;
using LyricDb.Web.Models.Dao;
using Riok.Mapperly.Abstractions;

namespace LyricDb.Web.Models.Dto.Responses;

public class SongInfoResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "<Untitled>";
    public string Artists { get; set; } = "<Unknown>";
    public string Album { get; set; } = "<Unknown>";
    public required UserInfoResponse Submitter { get; set; }
    public List<Guid> Lyrics { get; set; } = [];
    public Guid CurrentLyric { get; set; } = default;
    public string? Cover { get; set; }
    public DateTime CreateTime { get; set; }
    public List<string> Binds { get; set; } = [];
}

[Mapper]
public partial class SongToSongInfoResponseMapper : IMapper<Song,SongInfoResponse>
{
    public partial SongInfoResponse Map(Song from);
    
}