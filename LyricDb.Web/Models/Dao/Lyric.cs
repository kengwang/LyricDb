using System.ComponentModel.DataAnnotations;

namespace LyricDb.Web.Models.Dao;

public class Lyric
{
    [Key]
    public required Guid Id { get; set; } = Guid.NewGuid();
    public required string Content { get; set; }
    public required DateTime CreateTime { get; set; }
    public required User Submitter { get; set; }
    public required Song Song { get; set; }
    public User? Reviewer { get; set; }
    public bool Approved { get; set; }
}