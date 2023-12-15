using Discord;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace Rozhok.Features.Configs.Classes;
public class ClientStatusSettings
{
    [YamlMember(Description = "Текст, который будет отображен в статусе клиента.")]
    public string ClientTextStatus { get; set; } = "симфонию атомов..";
    [YamlMember(Description = "Тип статуса: Playing: \"Играет\", Watching: \"Стримит\" (только твич), Listening: \"Слушает\", Watching: \"Смотрит\", Competing: \"Соревнуется в\", CustomStatus: \"кастомный статус, без текста в начале.\"")]
    public ActivityType TypeOfStatus { get; set; } = ActivityType.Listening;
    [YamlMember(Description = "Устанавливает ссылку на твитч, если тип статуса бота - стримит")]
    public string TwitchURL { get; set; } = "";
    [YamlMember(Description = "Устанавилвает статус в дискорде. Существуют: Offline, Online, Idle, AFK, DoNotDisturb, Invisible. Переводите сами, мне лень..")]
    public UserStatus ClientUserStatus { get; set; } = UserStatus.Idle;
}