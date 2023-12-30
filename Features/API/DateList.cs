using Discord;
using Rozhok.Features.Configs;
using Image = System.Drawing.Image;
using System.Drawing;

namespace Rozhok.API;

public static class DateList
{
    public static Emoji Checkmark = new Emoji("✅");

    private static Bitmap _dem_static;
    public static Bitmap DemotivatorStatic = _dem_static ??= GetDemotivatorImage();
    public static Bitmap GetDemotivatorImage()
    {
        Console.WriteLine("Начинаем загрузку шаблона для демотиватора..");

        string path = Path.Combine(ConfigsLoader.GetPath("Assets"), "demotivator.png");

        if (!File.Exists(path))
            Extensions.RawDownloadAndSaveFile("https://cdn.discordapp.com/attachments/934101013458194533/1185222279345488013/demotivator.png",
                path);


        Image img = Image.FromFile(path);
        new Bitmap(img, img.Width, img.Height);

        return new Bitmap(img, img.Width, img.Height);
    }
}
