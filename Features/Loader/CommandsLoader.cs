namespace Rozhok.Features.Commands;

using Discord;
using Discord.WebSocket;
using Rozhok.Features.Configs;
using System;
using System.Reflection;

public class CommandsLoader
{
    public static List<Command> Commands { get; } = new();
    public static void RegisterCommands()
    {
        foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
        {
            if (type.IsInterface || type.IsAbstract) continue;

            if (type.BaseType != typeof(Command)) continue;

            ConstructorInfo ctr = type.GetConstructor(Type.EmptyTypes)!;
            Command module = (ctr.Invoke(null) as Command)!;

            Commands.Add(module!);
        }
    }

    public static async void ExecuteCommand(SocketMessage msg, string commandName, string[] args, bool IsDeveloper = false)
    {
        Command? Command = Commands.FirstOrDefault(x =>
        x.Name.ToLower() == commandName.ToLower()
        || x.Aliases.Any(x => x.ToLower() == commandName.ToLower()));

        if (Command == null) return;

        if (!IsDeveloper && Command.IsDeveloperCommand)
        {
            await msg.Channel.SendMessageAsync(ConfigsLoader.Config.CommandsSettings.DeniedMessage);
            return;
        }

        try
        {
            Command.Execute(msg, args, IsDeveloper);
        } catch(Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}