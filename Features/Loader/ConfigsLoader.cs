namespace Rozhok.Features.Configs;

using System;
using YamlDotNet.Serialization;

public class ConfigsLoader
{
    private static Config? _config;
    public static Config Config => _config ??= LoadConfig();

    private static string ConfigPath = GetPath("config.yml");

    private static readonly ISerializer Serializer = new SerializerBuilder().Build();
    private static readonly IDeserializer Deserializer = new DeserializerBuilder().Build();

    private static string _ImageDirectory;
    public static readonly string ImageDirectory = _ImageDirectory ??= GetPath("Images");

    private static string _DataDirectory;
    public static readonly string DataDirectory = _DataDirectory ??= GetPath("Data");
    public static string GetPath(string path) => Path.Combine(DirectoryPath, path);
    public const string DirectoryPath = "Rozhok.Data";
    public static Config LoadConfig()
    {
        Console.WriteLine("Загружаем конфиг..");

        if (!Directory.Exists(DirectoryPath)) Directory.CreateDirectory(DirectoryPath);

        if (File.Exists(ConfigPath))
        {
            Console.WriteLine("Попался! Читаем активный конфиг..");

            Config deserializedConfig = Deserializer.Deserialize<Config>(File.ReadAllText(ConfigPath));

            File.WriteAllText(ConfigPath, Serializer.Serialize(deserializedConfig).ToString());


            _config = deserializedConfig;
            return deserializedConfig;
        }
        else
        {
            Config DefaultConfig = new();

            Console.WriteLine("Привет! Создаю для вас конфиг, пожалуйста, укажите ваш токен бота. (вставить в консоль - правая кнопка мыши)");

            string ptoken = Console.ReadLine()!;

            if (ptoken != string.Empty) DefaultConfig.Token = ptoken!;

            string serializedConfig = Serializer.Serialize(DefaultConfig);

            File.WriteAllText(ConfigPath, serializedConfig.ToString());

            _config = DefaultConfig;
            return DefaultConfig;
        }
    }
}