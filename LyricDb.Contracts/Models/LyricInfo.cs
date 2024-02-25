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
    public bool Approved { get; set; }
}