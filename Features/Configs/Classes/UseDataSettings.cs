using YamlDotNet.Serialization;

namespace Rozhok.Features.Configs.Classes;
public class UseDataSettings
{
    [YamlMember(Description = "ID канала/ов, куда будут писаться сообщения при пинге/случайном сообщении и читаться оттуда.")]
    public List<ulong> IDChannelsToSaveAndWrite { get; set; } = new() { 1, 2 };
    [YamlMember(Description = "Устанавливает появится сообщения без упоминания бота или нет.")]
    public bool RandomMessage { get; set; } = true;
    [YamlMember(Description = "Шанс на сообщение (в %, по умолчанию - 25%, знак % не нужно добавлять)")]
    public int MessageChance { get; set; } = 25;

}