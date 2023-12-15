using Discord.WebSocket;
using Rozhok.API;
using Rozhok.Features.Configs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rozhok.Features.Commands;

public class Delete : Command
{
    public override string Name => "delete";
    public override string Description => "Удалить изображения и/или текст в указанном сообщении.";
    public override bool IsDeveloperCommand => true; 
    public override async void Execute(SocketMessage message, string[] args, bool IsDeveloper)
    {
        if(args.Count() < 1)
        {
            await message.Channel.SendMessageAsync("Укажите тип того, чего именно вы хотите удалить в сообщении из базы-данных:\nimages - изображения\ntext - текст\nall или любой другой аргумент - все");
            return;
        }

        Type type = Type.None;
        if (args[0] == "image" || args[0] == "img" || args[0] == "images") type = Type.Images;
        else if(args[0] == "text" || args[0] == "txt" || args[0] == "texts") type = Type.Text;
        
        if (message.Reference != null)
        {
            var replymsg = await message.Channel.GetMessageAsync(message.Reference.MessageId.Value);

            if(replymsg.Author.Id != Bot.Client?.CurrentUser.Id)
            {
                await message.Channel.SendMessageAsync("Пожалуйста, ответьте на **мое** сообщение, которое вы хотите удалить из базы-данных");
                return;
            }

            if (replymsg.Attachments.Count > 0 && (type == Type.Images || type == Type.None))
            {
                int count = 0;
                foreach (var file in Directory.GetFiles(Path.Combine(ConfigsLoader.GetPath("Images"))))
                {
                    if (file.EndsWith(replymsg.Attachments.FirstOrDefault()!.Filename))
                    {
                        count++;
                        File.Delete(file);
                    }
                }
                await message.Channel.SendMessageAsync($"Удалено {count} изображения/й по вашему запросу");
            }

            if(replymsg.Content != null && (type == Type.Text || type == Type.None))
            {
                string path = "";
                if (ConfigsLoader.Config.SaveDataSettings.OurFile)
                {
                    path = ConfigsLoader.GetPath(Path.Combine("Data", "data.txt"));
                }
                else
                {
                    path = ConfigsLoader.GetPath(Path.Combine("Data", $"{(message.Channel as SocketGuildChannel)?.Guild.Id}_data.txt"));
                }

                int count = 0;
                foreach (string line in File.ReadAllText(path).Split('\n'))
                {
                    if(line.Contains(replymsg.Content))
                    {
                        line.Replace(replymsg.Content, "[ОТРЕДАКТИРОВАНО]");
                        count++;
                    }
                }
                //File.WriteAllText(path, File.ReadAllText(path).Replace(replymsg.Content, "[ОТРЕДАКТИРОВАНО]"));

                await message.Channel.SendMessageAsync($"Удалено {count} совпадения/й по вашему запросу");
            }

            if (type == Type.Text || type == Type.None)
            {
                await message.Channel.SendMessageAsync($"Обновление сообщений начато. Если оригинальное сообщение удалено - процесс успешен.");

                SaveAndWrite.LoadData((replymsg.Channel as SocketGuildChannel)!.Guild.Id);
            }

            await replymsg.DeleteAsync();
            return;
        }
        else await message.Channel.SendMessageAsync("Пожалуйста, ответьте на мое сообщение, которое вы хотите удалить из базы-данных");
    }

    enum Type
    {
        None,
        Images,
        Text
    }
}
