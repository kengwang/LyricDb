namespace LyricDb.Web.Models.Dto.Requests;

public class LyricCreateRequest
{
    public Guid SongId { get; set; }
    public required string Content { get; set; }
    public string? Author { get; set; }
    public string? Translator { get; set; }
    public string? Transliterator { get; set; }
    public string? Timeline { get; set; }
    public string? Proofreader { get; set; }
}