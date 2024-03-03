using System;

namespace LyricDb.Contracts.Models;

public class LyricInfo
{
    public required Guid Id { get; set; } = Guid.NewGuid();
    public required string Content { get; set; }
    public required DateTime CreateTime { get; set; }
    public required UserInfo Submitter { get; set; }
    public required SongInfo Song { get; set; }
    public UserInfo? Reviewer { get; set; }
    public LyricStatus Status { get; set; }
    public string? Author { get; set; }
    public string? Translator { get; set; }
    public string? Transliterator { get; set; }
    public string? Timeline { get; set; }
    public string? Proofreader { get; set; }
}

public enum LyricStatus
{
    Pending,
    Approved,
    Rejected
}