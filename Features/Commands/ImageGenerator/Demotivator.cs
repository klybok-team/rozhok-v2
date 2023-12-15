using Blazor.Extensions.Canvas.Canvas2D;
using Discord;
using Discord.WebSocket;
using Rozhok.API;
using Rozhok.Features.Configs;
using System;
using System.Collections.Generic;
using Image = System.Drawing.Image;
using System.Drawing;
using ImageFormat = System.Drawing.Imaging.ImageFormat;
using System.Net;
using static System.Net.Mime.MediaTypeNames;

namespace Rozhok.Features.Commands;

public class Demotivator : Command
{
    public override string Name => "demotivator";

    public override string[] Aliases => new string[] { "dem", "d" };

    private static StringFormat StringFormatMiddle = new StringFormat(StringFormatFlags.NoClip)
    {
        Alignment = StringAlignment.Center,
    };
    public override async void Execute(SocketMessage msg, string[] args, bool IsDev)
    {
        string subtitle = string.Join(" ", args).Split(" | ")[0] ?? "";
        string subtitle2 = string.Join(" ", args).Split(" | ")[1] ?? "";

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

            var replymsg = await msg.Channel.GetMessageAsync(msg.Reference.MessageId.Value);

            if(replymsg.Attachments.Count < 1)
            {
                await msg.Channel.SendMessageAsync("В указанном сообщении отсутствуют изображения.");
                return;
            }

            AttachmentUrl = replymsg.Attachments.First().Url;
        }

        Image img;
        using (var client = new WebClient())
        {
            var content = client.DownloadData(AttachmentUrl);
            using (var stream = new MemoryStream(content))
            {
                try
                {
                    img = Image.FromStream(stream);
                } catch
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

            g.DrawString(subtitle,
                new Font("Lobster", 20),
                Brushes.White,
                // это гениально
                new PointF(img.Width / 2, standartHeight), StringFormatMiddle);

            if (subtitle2 != string.Empty)
            {
                g.DrawString(subtitle2,
                new Font("Lobster", 20),
                Brushes.White,
                new PointF(img.Width / 2, standartHeight + 33), StringFormatMiddle);
            }
        }
        img = bmp;

        var ms = new MemoryStream();
        img.Save(ms, ImageFormat.Png);
        ms.Position = 0;

        await msg.Channel.SendFileAsync(ms, "demotivator.png", "Ваше изображение:");

        img.Dispose();
    }
}
