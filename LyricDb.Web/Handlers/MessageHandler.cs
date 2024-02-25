using LyricDb.Contracts.Models;
using LyricDb.Web.Interfaces;

namespace LyricDb.Web.Handlers;

public class MessageHandler
{
    public async Task Handle(Message message, IRepository<Message> repository)
    {
        await repository.CreateAsync(message);
    }
}