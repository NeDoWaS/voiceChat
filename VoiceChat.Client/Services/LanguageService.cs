namespace VoiceChat.Client.Services;

public class LanguageService
{
    public string CurrentLanguage { get; private set; } = "en";

    public event Action? OnLanguageChanged;

    private readonly Dictionary<string, Dictionary<string, string>> _translations = new()
    {
        ["en"] = new()
        {
            ["RoomStatus"] = "Available",
            ["Dashboard"] = "Dashboard",
            ["Rooms"] = "Rooms",
            ["Settings"] = "Settings",
            ["Welcome"] = "Welcome back!",
            ["Subtitle"] = "Choose a room and start talking.",
            ["CreateRoom"] = "Create room",
            ["YourRooms"] = "Available rooms",
            ["Refresh"] = "Refresh",
            ["NoRooms"] = "No rooms yet",
            ["NoRoomsDescription"] = "Create your first room and invite others.",
            ["RoomName"] = "Room name",
            ["Description"] = "Description",
            ["Create"] = "Create",
            ["Cancel"] = "Cancel",
            ["Loading"] = "Loading rooms...",
            ["Profile"] = "Profile",
            ["Logout"] = "Log out",
            ["Join"] = "Join room",
            ["NameRequired"] = "Room name is required."
        },

        ["uz"] = new()
        {
            ["RoomStatus"] = "Mavjud",
            ["Dashboard"] = "Bosh sahifa",
            ["Rooms"] = "Xonalar",
            ["Settings"] = "Sozlamalar",
            ["Welcome"] = "Xush kelibsiz!",
            ["Subtitle"] = "Xonani tanlang va suhbatni boshlang.",
            ["CreateRoom"] = "Xona yaratish",
            ["YourRooms"] = "Mavjud xonalar",
            ["Refresh"] = "Yangilash",
            ["NoRooms"] = "Hozircha xonalar yo'q",
            ["NoRoomsDescription"] = "Birinchi xonangizni yarating va boshqalarni taklif qiling.",
            ["RoomName"] = "Xona nomi",
            ["Description"] = "Tavsif",
            ["Create"] = "Yaratish",
            ["Cancel"] = "Bekor qilish",
            ["Loading"] = "Xonalar yuklanmoqda...",
            ["Profile"] = "Profil",
            ["Logout"] = "Chiqish",
            ["Join"] = "Xonaga kirish",
            ["NameRequired"] = "Xona nomini kiriting."
        },

        ["ru"] = new()
        {   ["RoomStatus"] = "Доступно",
            ["Dashboard"] = "Главная",
            ["Rooms"] = "Комнаты",
            ["Settings"] = "Настройки",
            ["Welcome"] = "С возвращением!",
            ["Subtitle"] = "Выберите комнату и начните общение.",
            ["CreateRoom"] = "Создать комнату",
            ["YourRooms"] = "Доступные комнаты",
            ["Refresh"] = "Обновить",
            ["NoRooms"] = "Пока нет комнат",
            ["NoRoomsDescription"] = "Создайте первую комнату и пригласите других.",
            ["RoomName"] = "Название комнаты",
            ["Description"] = "Описание",
            ["Create"] = "Создать",
            ["Cancel"] = "Отмена",
            ["Loading"] = "Загрузка комнат...",
            ["Profile"] = "Профиль",
            ["Logout"] = "Выйти",
            ["Join"] = "Войти в комнату",
            ["NameRequired"] = "Введите название комнаты."
        }
    };

    public void SetLanguage(string language)
    {
        if (!_translations.ContainsKey(language))
            return;

        CurrentLanguage = language;
        OnLanguageChanged?.Invoke();
    }

    public string Get(string key)
    {
        if (_translations.TryGetValue(CurrentLanguage, out var translations) &&
            translations.TryGetValue(key, out var value))
        {
            return value;
        }

        return key;
    }
}