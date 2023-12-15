using Discord.WebSocket;

namespace Rozhok.Features.Commands;

public interface ICommand
{
    string Name { get; }
    string[] Aliases { get; }
    string Description { get; }
    void Execute(SocketMessage msg, string[] args, bool IsDeveloper);
    bool IsDeveloperCommand { get; }
}