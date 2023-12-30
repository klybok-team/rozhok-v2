using YamlDotNet.Serialization;

namespace Rozhok.Features.Configs.Classes;
public class SaveDataSettings
{
    [YamlMember(Description = "Сохранять ли ВСЕ данные?")]
    public bool SaveAnyData { get; set; } = true;
    [YamlMember(Description = "Сохранять & использовать изображения?")]
    public bool ImagesSaveAndUse { get; set; } = true;
    [YamlMember(Description = "Общий ли файл с текстом для разных гильдий?\nЕсли нет: файлы делятся на ${айди гильдии}_data.txt.\nЕсли да: все хранится в одном файле.")]
    public bool OurFile { get; set; } = false;
    [YamlMember(Description = "Настройка сохранения данных:\nЕсли пользователь отправил указанное кол-во (или >) изображений в одном сообщении, они не сохраняются, лимит - 10")]
    public int LimitImagesToOnce { get; set; } = 3;
    [YamlMember(Description = "Если в сообщении > указанного кол-ва символов, бот не будет записывать сообщение в файл.")]
    public int MaxLenghtToWrite { get; set; } = 100;
    [YamlMember(Description = "Лимит на количество сохраненных изображений.")]
    public int LimitImagesInFolder { get; set; } = 100;
    [YamlMember(Description = "Определяет способ фильтрации сообщений. На данный момент имеется 2 типа: None, Links")]
    public MessageFilter MessageFilterType { get; set; } = MessageFilter.None;
}
public enum MessageFilter
{
    None,
    Links,
}