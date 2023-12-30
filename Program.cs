using Rozhok;
using Rozhok.Features.Configs;

public class Program
{
    private static Bot? _bot;
    public static Version Version { get; set; } = new(2, 0, 0);
    public static Task Main(string[] args)
    {
        Console.Title = $"rozhok v{Version}";
        Console.ForegroundColor = ConsoleColor.White;

        Console.WriteLine("Создаем тело бота..");

        _bot = new Bot(ConfigsLoader.Config.Token);

        AppDomain.CurrentDomain.ProcessExit += (_, _) =>
        {
            Rozhok.API.SaveAndWrite.WriteData();

            _bot.Destroy();
        };

        Console.WriteLine("Отлично! Начинаем процесс включения бота..");

        _bot.Login();

        while (true)
        {
            string? s = Console.ReadLine();

            if (s == null) continue;

            switch (s)
            {
                default:
                    Console.WriteLine("Привет, не узнал твою команду! Разбираешься с консолью? - введи help");
                    break;
                case "help":
                    Console.WriteLine("Список команд:\nconfig - перечитывает конфиг\nbot - перезапускает бота\nsave - сохраняет данные\nfilter - избавить текст от пустых строк");
                    break;
                case "config":
                    ConfigsLoader.LoadConfig();
                    break;
                case "bot":
                    _bot.Destroy();
                    _bot.Login();
                    break;
                case "save":
                    Rozhok.API.SaveAndWrite.WriteData();
                    break;
                case "filter":
                    Rozhok.API.SaveAndWrite.FilterFile();
                    break;
            }
        }
    }

}