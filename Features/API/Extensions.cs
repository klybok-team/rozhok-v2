using Rozhok.Features.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rozhok.Features.Configs.Classes;
using System.Text.RegularExpressions;
using Discord.WebSocket;
using System.IO;
using System.Net;
using Discord;

namespace Rozhok.API;

public static class Extensions
{
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
}
