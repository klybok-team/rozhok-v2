using YamlDotNet.Serialization;

namespace Rozhok.Features.Configs.Classes;
public class CommandsSettings
{
    [YamlMember(Description = "Устанавливает включены команды или нет.")]
    public bool IsEnabled { get; set; } = true;
    [YamlMember(Description = "Устанавливает префикс команд.")]
    public string Prefix { get; set; } = "roz.";
    [YamlMember(Description = "Устанавливает людей, которые могут пользоватся DEV-командами.")]
    public List<ulong> DeveloperAccess { get; set; } = new();
    [YamlMember(Description = "Устанавливает ответ если в доступе к DEV-командам отказано.")]
    public string DeniedMessage { get; set; } = "У вас нет прав на использование этой команды.";
}