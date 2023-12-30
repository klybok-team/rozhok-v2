namespace Rozhok.Features.Configs;

using Classes;
using YamlDotNet.Serialization;

public class Config
{
    [YamlMember(Description = "Определяет токен бота.")]
    public string Token { get; set; } = "";

    [YamlMember(Description = "Настройки конфига сохранения данных.")]
    public SaveDataSettings SaveDataSettings { get; set; } = new();
    [YamlMember(Description = "Настройки конфига использования данных.")]
    public UseDataSettings UseDataSettings { get; set; } = new();
    [YamlMember(Description = "Настройки конфига команд.")]
    public CommandsSettings CommandsSettings { get; set; } = new();
    [YamlMember(Description = "Настройки статуса бота.")]
    public ClientStatusSettings ClientStatusSettings { get; set; } = new();
}