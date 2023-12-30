using Discord.WebSocket;

namespace Rozhok.Features.Commands;

public abstract class Command : ICommand
{
    public virtual string Name => "null";
    public virtual string[] Aliases => Array.Empty<string>();
    public virtual bool IsDeveloperCommand => false;
    public virtual string Description => "null";
    public virtual void Execute(SocketMessage msg, string[] args, bool IsDeveloper)
    {
        Console.WriteLine($"{msg.Author.Username} выполнил команду: {Name}");
    }
}
