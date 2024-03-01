using Emoji = Discord.Emoji;
using Rozhok.Features.Configs;
using SixLabors.ImageSharp;
using SixLabors.Fonts;

namespace Rozhok.API;

public static class DateList
{
    public static Emoji Checkmark = new Emoji("✅");

    private static Image _dem_static;
    public static Image DemotivatorStatic = _dem_static ??= GetDemotivatorImage();
    public static Image GetDemotivatorImage()
    {
        Console.WriteLine("Начинаем загрузку шаблона для демотиватора..");

        string path = Path.Combine(ConfigsLoader.GetPath("Assets"), "demotivator.png");

        if (!File.Exists(path))
            Extensions.RawDownloadAndSaveFile("https://cdn.discordapp.com/attachments/934101013458194533/1185222279345488013/demotivator.png",
                path);

        return Image.Load(path);
    }
    // https://cdn.discordapp.com/attachments/934101013458194533/1191083779297640548/Lobster.ttf
    private static Font _lobster_font;
    public static Font LobsterFont = _lobster_font ??= GetLobsterFont();
    public static Font GetLobsterFont()
    {
        Console.WriteLine("Начинаем загрузку шаблона для шрифта..");

        string path = Path.Combine(ConfigsLoader.GetPath("Assets"), "Lobster.ttf");

        if (!File.Exists(path))
            Extensions.RawDownloadAndSaveFile("https://cdn.discordapp.com/attachments/934101013458194533/1191083779297640548/Lobster.ttf",
                path);

        FontFamily family = new FontCollection().Add(path);
        return family.CreateFont(20, FontStyle.Italic);
    }
}
