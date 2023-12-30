using Rozhok.Features.Configs;
using Discord.WebSocket;
using System.Net;
using Discord;

namespace Rozhok.API;

public static class Extensions
{
    public static Random Random = new Random();
    public static void DownloadAndSaveFile(string url, SocketMessage data, string filename)
    {
        if (!Directory.Exists(ConfigsLoader.ImageDirectory)) Directory.CreateDirectory(ConfigsLoader.ImageDirectory);

        if (Directory.GetFiles(ConfigsLoader.ImageDirectory).Count() >= ConfigsLoader.Config.SaveDataSettings.LimitImagesInFolder) return;

#pragma warning disable
        using (WebClient client = new WebClient())
        {
            client.DownloadFile(new Uri(url), Path.Combine(ConfigsLoader.ImageDirectory, $"{data.Id}_{filename}"));
        }
#pragma warning enable

        Console.WriteLine($"Скачано изображение: {data.Id}_{filename}");
    }
    public static void RawDownloadAndSaveFile(string url, string path)
    {
#pragma warning disable
        using (WebClient client = new WebClient())
        {
            client.DownloadFile(new Uri(url), path);
        }
#pragma warning enable
    }
    public static ulong GetGuildId(this ISocketMessageChannel socket)
    {
        return (socket as SocketGuildChannel)!.Guild.Id;
    }
    public static ITextChannel GetTextChannel(this ISocketMessageChannel socket)
    {
        return (socket as ITextChannel);
    }
    public static T RandomItem<T>(this T[] array)
    {
        return array[Random.Next(0, array.Length)];
    }
    public static T RandomItem<T>(this List<T> list)
    {
        return list[Random.Next(0, list.Count)];
    }

    public static T PullRandomItem<T>(this List<T> list)
    {
        int index = Random.Next(0, list.Count);
        T result = list[index];
        list.RemoveAt(index);
        return result;
    }
}
