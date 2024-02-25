using LyricDb.Contracts.Messages;
using LyricDb.Contracts.Models;
using Wolverine;

namespace LyricDb.Worker.Handlers;

public class LyricAddHandler
{
    public async Task Handle(LyricAddedMessage message, IMessageContext context)
    {
        // send message to reviewers and admin
        await context.RespondToSenderAsync(new Message
        {
            Id = Guid.NewGuid(),
            TargetRole = 2,
            TargetUserIds = [],
            Content = $"新的歌词已经添加, 你可以前往: https://lyricdb.kengwang.com.cn/lyric/{message.Lyric.Id} 查看",
            CreatedAt = DateTime.Now,
            ReadUsers = []
        });
    }
}