using System;
using LyricDb.Contracts.Models;

namespace LyricDb.Contracts.Messages;

public class LyricAddedMessage
{
    public SongInfo Song { get; set; }
    public LyricInfo Lyric { get; set; }
    public UserInfo User { get; set; }
}