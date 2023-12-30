using Discord;
using Discord.WebSocket;
using Rozhok.API;
using Rozhok.Features.Commands;
using Rozhok.Features.Configs;
using System.Text.RegularExpressions;

namespace Rozhok;

public class Bot
{
    public static DiscordSocketClient? Client;
    private string Token;

    public Bot(string token)
    {
        Token = token;
    }

    public async void Destroy()
    {
        Console.WriteLine("Закругляемся..");

        if (Client != null)
        {
            await Client!.StopAsync();
            await Client.LogoutAsync();

            Client = null;
        }

        await Task.Delay(1000);
    }
    public async void Login()
    {
        Client = new DiscordSocketClient(new DiscordSocketConfig()
        {
            GatewayIntents = GatewayIntents.All,
            LogGatewayIntentWarnings = false,
        });

        Client.Ready += OnReady;
        Client.MessageReceived += OnMessageReceived;

        await Client.LoginAsync(TokenType.Bot, Token);
        await Client.StartAsync();

        CommandsLoader.RegisterCommands();

        Console.WriteLine("Подключаемся к дискорду. Если подключения нет долгое время - токен указан неправильно.");
    }

    private Task OnMessageReceived(SocketMessage message)
    {
        if (message.Author.IsBot || message.Channel.GetChannelType() == ChannelType.DM) return Task.CompletedTask;

        if (message.Content.StartsWith(ConfigsLoader.Config.CommandsSettings.Prefix))
        {
            string[] args = Regex.Replace(message.Content, @"^roz\.", "", RegexOptions.None).Split(" ");
            string? nameOfCommand = args.FirstOrDefault();

            // Убирает 1 аргумент
            args = args[1..];

            CommandsLoader.ExecuteCommand(
                message,
                nameOfCommand!,
                args,
                ConfigsLoader.Config.CommandsSettings.DeveloperAccess.Contains(message.Author.Id));

            return Task.CompletedTask;
        }

        if (!ConfigsLoader.Config.UseDataSettings.IDChannelsToSaveAndWrite.Contains(message.Channel.Id)) return Task.CompletedTask;

        SaveAndWrite.TrySaveData(message);

        if (message.MentionedUsers.Any(x => x.Id == Client?.CurrentUser.Id)
            || ConfigsLoader.Config.UseDataSettings.RandomMessage
            && ConfigsLoader.Config.UseDataSettings.MessageChance <= Extensions.Random.Next(0, 100))
        {
            SaveAndWrite.GetRandomMessage(message);
        }

        return Task.CompletedTask;
    }

    private async Task OnReady()
    {
        Console.WriteLine($"Готов,\nИмя: {Client?.CurrentUser.Username}#{Client?.CurrentUser.Discriminator}\n\nДОП. ИНФ.\nАйди клиента: {Client?.CurrentUser.Id}\nКоличество серверов: {Client?.Guilds.Count}");

        Console.WriteLine(GetCategoryNotification());

        Console.ForegroundColor = ConsoleColor.White;

        await Client!.SetStatusAsync(ConfigsLoader.Config.ClientStatusSettings.ClientUserStatus);

        await Client.SetGameAsync(ConfigsLoader.Config.ClientStatusSettings.ClientTextStatus,
            ConfigsLoader.Config.ClientStatusSettings.TwitchURL,
            ConfigsLoader.Config.ClientStatusSettings.TypeOfStatus);

        DateList.GetDemotivatorImage();
    }

    enum ImageType
    {
        Avalible,
        Full
    }
    string GetCategoryNotification()
    {
        ImageType type = ImageType.Avalible;

        if (Directory.GetFiles(ConfigsLoader.ImageDirectory).Count()
            >= ConfigsLoader.Config.SaveDataSettings.LimitImagesInFolder)
            type = ImageType.Full;

        if (type == ImageType.Full)
        {
            Console.ForegroundColor = ConsoleColor.Red;

            return $"Количество изображений в папке для их хранения превышает лимит. ({Directory.GetFiles(ConfigsLoader.ImageDirectory).Count()} из {ConfigsLoader.Config.SaveDataSettings.LimitImagesInFolder})";
        }

        Console.ForegroundColor = ConsoleColor.Cyan;

        return $"{Directory.GetFiles(ConfigsLoader.ImageDirectory).Count()} из {ConfigsLoader.Config.SaveDataSettings.LimitImagesInFolder} места свободно для изображений в папке их хранения.";
    }
}