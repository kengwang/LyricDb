using System.ComponentModel.DataAnnotations;
using LyricDb.Contracts.Models;

namespace LyricDb.Web.Models.Dao;

public class Lyric
{
    [Key] public required Guid Id { get; set; } = Guid.NewGuid();
    public required string Content { get; set; }
    public required DateTime CreateTime { get; set; }
    public required User Submitter { get; set; }
    public required Song Song { get; set; }
    public User? Reviewer { get; set; }
    public LyricStatus Status { get; set; }
    public string? Author { get; set; }
    public string? Translator { get; set; }
    public string? Transliterator { get; set; }
    public string? Timeline { get; set; }
    public string? Proofreader { get; set; }
}