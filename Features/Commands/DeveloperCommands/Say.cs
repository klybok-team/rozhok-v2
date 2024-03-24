using Discord.WebSocket;

namespace Rozhok.Features.Commands;

public class Say : Command
{
    public override string Name => "say";
    public override string Description => "Сказать что-то.";
    public override bool IsDeveloperCommand => true;
    public override string[] Aliases => Array.Empty<string>();
    public override async void Execute(SocketMessage message, string[] args, bool IsDeveloper)
    {
        if (args.Count() < 1)
        {
            await message.Channel.SendMessageAsync("Скажите что-то.");
        }
        await message.Channel.SendMessageAsync(string.Join(" ", args));
    }
}
