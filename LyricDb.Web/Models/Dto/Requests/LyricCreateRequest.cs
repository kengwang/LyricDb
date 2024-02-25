namespace LyricDb.Web.Models.Dto.Requests;

public class LyricCreateRequest
{
    public Guid SongId { get; set; }
    public required string Content { get; set; }
}