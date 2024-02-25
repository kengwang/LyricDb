namespace LyricDb.Web.Models.Dto.Requests;

public class SongPostRequest
{
    public string Name { get; set; } = "<Untitled>";
    public string Artists { get; set; } = "<Unknown>";
    public string Album { get; set; } = "<Unknown>";
    public List<string> Binds { get; set; } = [];
}

public class SongPutRequest : SongPostRequest
{
    public Guid CurrentLyric { get; set; }
}