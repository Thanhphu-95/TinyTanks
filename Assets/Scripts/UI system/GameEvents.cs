using System; // Thư viện chứa Action

public static class GameEvents
{
    
    public static Action<string> OnMapSelected;// Sự kiện khi người chơi chọn một màn chơi (truyền vào tên Scene)
    public static Action<bool> OnToggleInGameMenu;
    public static Action<bool> OnTogglePause; // sự kiện spause game

    public static void RaiseMapSelected(string sceneName)// Hàm tiện ích để kích hoạt sự kiện này
    {
        OnMapSelected?.Invoke(sceneName); // Gửi thông báo đến tất cả những ai đang nghe
    }

    
}