using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LyricDb.Web.Models.Dao;

public class Song
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "<Untitled>";
    public string Artists { get; set; } = "<Unknown>";
    public string Album { get; set; } = "<Unknown>";
    public string? Cover { get; set; }
    public required User Submitter { get; set; }
    public required DateTime CreateTime { get; set; }
    public int Duration { get; set; }
    public Guid CurrentLyric { get; set; } = default;
    public List<Guid> Lyrics { get; set; } = [];
    public List<string> Binds { get; set; } = [];
}