using Discord;
using Discord.WebSocket;
using Rozhok.API;
using Image = System.Drawing.Image;
using System.Drawing;
using ImageFormat = System.Drawing.Imaging.ImageFormat;
using System.Net;

namespace Rozhok.Features.Commands;

public class Demotivator : Command
{
    public override string Name => "demotivator";
    public override string[] Aliases => new string[] { "dem", "d" };
    public override async void Execute(SocketMessage msg, string[] args, bool IsDev)
    {
        await msg.Channel.SendMessageAsync("Команда отключена.");
        return;

        string subtitle = "";
        string subtitle2 = "";

        if (args.Count() < 1)
        {
            await msg.Channel.SendMessageAsync("Нас 25 тысяч и мы идем разбираться, где блять текст??");
            return;
        }
        else
        {
            string[] splited_args = string.Join(" ", args).Split(" | ");

            if (splited_args.Count() > 0) subtitle = splited_args[0];
            else subtitle = string.Join(" ", args);
        }

        if (args.Count() > 1)
        {
            string[] splited_args = string.Join(" ", args).Split(" | ");

            if (splited_args.Count() > 1) subtitle2 = splited_args[1];
        }

        Console.WriteLine(subtitle);
        Console.WriteLine(subtitle2);

        string AttachmentUrl;
        if (msg.Attachments.Count > 0)
            AttachmentUrl = msg.Attachments.First().Url;
        else
        {
            if (msg.Reference == null)
            {
                await msg.Channel.SendMessageAsync("Пожалуйста, ответьте на сообщение или пришлите изображение из которого вы хотите сделать демотиватор.");
                return;
            }

            IMessage replymsg = await msg.Channel.GetMessageAsync(msg.Reference.MessageId.Value);

            if (replymsg.Attachments.Count < 1)
            {
                await msg.Channel.SendMessageAsync("В указанном сообщении отсутствуют изображения.");
                return;
            }

            AttachmentUrl = replymsg.Attachments.First().Url;
        }

        Image img;
        using (WebClient client = new WebClient())
        {
            byte[] content = client.DownloadData(AttachmentUrl);
            using (MemoryStream stream = new MemoryStream(content))
            {
                try
                {
                    img = Image.FromStream(stream);
                }
                catch
                {
                    await msg.Channel.SendMessageAsync("Формат изображения не поддерживается или произошла ошибка при обработке изображения.");
                    return;
                }
            }
        }

        Bitmap bmp = new Bitmap(DateList.DemotivatorStatic,
            DateList.DemotivatorStatic.Width,
            DateList.DemotivatorStatic.Height);

        using (Graphics g = Graphics.FromImage(bmp))
        {
            g.DrawImage(img, new Rectangle(33, 33, 446, 390));

            int standartHeight = 450;
            if (subtitle2 != string.Empty) standartHeight -= 20;

            Font lobsterFont = new Font("Lobster", 20);

            subtitle = CenteredString(subtitle, img.Width);
            subtitle2 = CenteredString(subtitle2, img.Width);

            SizeF size = g.MeasureString(subtitle, lobsterFont);
            SizeF size2 = g.MeasureString(subtitle2, lobsterFont);

            g.DrawString(subtitle,
                lobsterFont,
                Brushes.White,
                new PointF((img.Width / 2) + size.Width, standartHeight));

            if (subtitle2 != string.Empty)
            {
                g.DrawString(subtitle2,
                    lobsterFont,
                    Brushes.White,
                    // это гениально
                    new PointF((img.Width / 2) + size2.Width, standartHeight));
            }
        }
        img = bmp;

        MemoryStream ms = new MemoryStream();
        img.Save(ms, ImageFormat.Png);
        ms.Position = 0;

        await msg.Channel.SendFileAsync(ms, "demotivator.png", "Ваше изображение:");

        img.Dispose();
    }
    public static string CenteredString(string s, int width)
    {
        return s.PadLeft((width + s.Length) / 4).PadRight(width);
    }
}
