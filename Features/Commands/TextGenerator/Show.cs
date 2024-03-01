using Discord;
using Discord.WebSocket;

namespace Rozhok.Features.Commands;

public class ShowMessage : Command
{
    public override string Name => "showmessage";
    public override string[] Aliases => new string[] { "sm" };
    public override async void Execute(SocketMessage socket_msg, string[] args, bool IsDev)
    {
        if (socket_msg.Reference == null)
        {
            await socket_msg.Channel.SendMessageAsync("Указанное сообщение отсутствует.");
            return;
        }

        IMessage replymsg = await socket_msg.Channel.GetMessageAsync(socket_msg.Reference.MessageId.Value);
        
        if(replymsg == null || replymsg.Content == null || replymsg.Content == string.Empty)
        {
            await socket_msg.Channel.SendMessageAsync("В указанном сообщении не найден текст.");
            return;
        }

        await socket_msg.Channel.SendMessageAsync($"```{replymsg.Content.Replace(@"`","")}```");
    }
}
