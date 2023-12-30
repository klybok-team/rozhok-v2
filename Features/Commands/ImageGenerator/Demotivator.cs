using Discord;
using Discord.WebSocket;
using System.Drawing;
using System.Net;
using Color = System.Drawing.Color;
using Image = System.Drawing.Image;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

namespace Rozhok.Features.Commands;
// Dear thanks to: https://github.com/pashokitsme/demotivatorgenerator
public class Demotivator : Command
{
    public override string Name => "demotivator";
    public override string Description => "Создать смешной демотиватор. Для того что-бы разделять текст используйте \" | \"\nДобавьте 3-ий аргумент для активации супер-пупер секретного режима. Аргумент не будет учитоваться в итоговом варианте.";
    public override string[] Aliases => new string[] { "dem", "d" };
    private static readonly Font LobsterFont = new Font("Lobster", 20);
    private static StringFormat StringFormat = new StringFormat()
    {
        Alignment = StringAlignment.Center
    };
    public override async void Execute(SocketMessage msg, string[] args, bool IsDev)
    {
        string subtext = "";
        string undtext = "";

        if (args.Count() < 1)
        {
            await msg.Channel.SendMessageAsync("Аргументы отсутствуют.");
            return;
        }

        string[] splited_args = string.Join(" ", args).Split(" | ");

        if (splited_args == null || splited_args.Count() <= 0)
        {
            subtext = string.Join(" ", args);
        }
        else
        {
            subtext = splited_args[0];

            if (splited_args.Count() > 1) undtext = splited_args[1];
        }

        bool WhiteBackground = false;
        if (splited_args != null && splited_args.Count() > 2) WhiteBackground = true;

        string AttachmentUrl = string.Empty;

        if (msg.Attachments.Count > 0)
        {
            AttachmentUrl = msg.Attachments.First().Url;
        }
        else if (msg.Reference != null)
        {
            IMessage replymsg = await msg.Channel.GetMessageAsync(msg.Reference.MessageId.Value);

            if (replymsg.Attachments.Count < 1)
            {
                await msg.Channel.SendMessageAsync("В указанном сообщении отсутствуют изображения.");
                return;
            }

            AttachmentUrl = replymsg.Attachments.First().Url;
        }

        if (AttachmentUrl == string.Empty)
        {
            await msg.Channel.SendMessageAsync("Пожалуйста, ответьте на сообщение или пришлите изображение из которого вы хотите сделать демотиватор.");
            return;
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

        Bitmap bmp = new Bitmap(400, 400);

        using (Graphics g = Graphics.FromImage(bmp))
        {
            if (!WhiteBackground) g.Clear(Color.Black);

            Point position = Offset(CenterPoint(), new Point(0, -50));

            g.DrawImage(img, Rect(325, 250, position));

            Pen linePen = new Pen(Brushes.White) { Width = 1.5f };

            g.DrawLine(linePen,
                       Offset(position, new Point(-325 / 2 - 5, -250 / 2 - 5)),
                       Offset(position, new Point(325 / 2 + 5, -250 / 2 - 5)));

            g.DrawLine(linePen,
                       Offset(position, new Point(-325 / 2 - 5, -250 / 2 - 5)),
                       Offset(position, new Point(-325 / 2 - 5, 250 / 2 + 5)));

            g.DrawLine(linePen,
                       Offset(position, new Point(325 / 2 + 5, -250 / 2 - 5)),
                       Offset(position, new Point(325 / 2 + 5, 250 / 2 + 5)));

            g.DrawLine(linePen,
                       Offset(position, new Point(-325 / 2 - 5, 250 / 2 + 5)),
                       Offset(position, new Point(325 / 2 + 5, 250 / 2 + 5)));

            RectangleF textBox = RectF(325 + 325 / 2,
                                       250 / 2,
                                       Offset(position, new Point(0, 250 / 2 + 250 / 3)));

            g.DrawString($"{subtext}\n{undtext}",
                         LobsterFont,
                         Brushes.White,
                         textBox,
                         StringFormat);
        }
        img = bmp;

        MemoryStream ms = new MemoryStream();
        img.Save(ms, ImageFormat.Png);
        ms.Position = 0;

        await msg.Channel.SendFileAsync(ms, "demotivator.png", "Ваше изображение:");

        img.Dispose();
    }
    static Point CenterPoint(int? height = null, int? width = null)
    {
        height = height == null ? 400 : height;
        width = width == null ? 400 : width;

        return new Point((int)(height / 2), (int)(width / 2));
    }

    static Rectangle Rect(int width, int height, Point position)
    {
        return new Rectangle(position.X - width / 2, position.Y - height / 2, width, height);
    }
    static RectangleF RectF(float width, float height, Point position)
    {
        return new RectangleF(position.X - width / 2, position.Y - height / 2, width, height);
    }

    static Point Offset(Point point, Point offset)
    {
        point.X += offset.X;
        point.Y += offset.Y;

        return point;
    }
}
