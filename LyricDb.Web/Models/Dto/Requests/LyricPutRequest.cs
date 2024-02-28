using LyricDb.Web.Models.Dto.Responses;

namespace LyricDb.Web.Models.Dto.Requests;

public class LyricPutRequest
{
    public Guid Id { get; set; }
    public Guid SongId { get; set; }
    public string? Content { get; set; }
    public int Status { get; set; }
    public string? Author { get; set; }
    public required int Duration { get; set; }
    public string? Translator { get; set; }
    public string? Transliterator { get; set; }
    public string? Timeline { get; set; }
    public string? Proofreader { get; set; }
}