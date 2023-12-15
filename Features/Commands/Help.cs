using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rozhok.Features.Commands;

public class Help : Command
{
    public override string Name => "help";
    public override void Execute(SocketMessage socket_msg, string[] args, bool IsDev)
    {
        if(args.Count() > 0)
        {
            Command command = CommandsLoader.Commands.Find(x => x.Name == args[0])!;

            if (command != null)
            {
                if(command.IsDeveloperCommand && !IsDev) socket_msg.Channel.SendMessageAsync("Описание команды: {command.Description}\nТы думал что здесь что-то будет?");
                else socket_msg.Channel.SendMessageAsync($"Описание команды: {command.Description}");

                return;
            }

            socket_msg.Channel.SendMessageAsync("Такой команды нет.\nЕсли вы хотите узнать список команд введите эту команду без аргументов.");
            return;
        }

        List<string> NormalCommands = new();
        foreach (var value in CommandsLoader.Commands.Where(x => !x.IsDeveloperCommand)) NormalCommands.Add(value.Name);

        string msg = $"Привет! Я рожок.\nВот список доступных вам команд: {string.Join(", ", NormalCommands)}";

        if (IsDev)
        {
            List<string> DeveloperCommands = new();
            foreach (var value in CommandsLoader.Commands.Where(x => x.IsDeveloperCommand)) DeveloperCommands.Add(value.Name);

            msg += $"\n\nПОЛУЧЕН ДОСТУП К КАРТОЧКЕ DEV... СПИСОК ДОСТУПНЫХ ВАМ КОМАНД: {string.Join(", ", DeveloperCommands)}";
        }

        socket_msg.Channel.SendMessageAsync(msg);
    }
}
