using System; // Thư viện chứa Action
using System.Collections.Generic;

public static class GameEvents
{
    
    public static Action<string> OnMapSelected;// Sự kiện khi người chơi chọn một màn chơi (truyền vào tên Scene)
    public static Action<bool> OnToggleInGameMenu;
    public static Action<bool> OnTogglePause; // sự kiện spause game

    public static System.Action<int, int> OnPlayerHealthChanged; // sự kiện về máu player
    public static System.Action<int> OnBossHealthChanged; // sự kiện maus boss
    public static System.Action<float> OnQuestTimeUpdate; //sự kiện về time quest
    public static Action<string> OnQuestTextChanged; // nội dung nhiệm vụ
  


    public static void RaiseMapSelected(string sceneName)// Hàm tiện ích để kích hoạt sự kiện này
    {
        OnMapSelected?.Invoke(sceneName); // Gửi thông báo đến tất cả những ai đang nghe
    }

    
    public static Action<bool> OnShieldStatusChanged;// Sự kiện thay đổi trạng thái khiên
    public static void RaiseShieldStatusChanged(bool hasShield)
    {
        OnShieldStatusChanged?.Invoke(hasShield);
    }
    
    public static Action<bool> OnHealthItemStatusChanged;// Sự kiện thay đổi trạng thái hồi máu
    public static void RaiseHealthItemStatusChanged(bool hasHealth)
    {
        OnHealthItemStatusChanged?.Invoke(hasHealth);
    }

    public static Action<bool> OnAcidBulletCountChanged;
    public static void RaiseAcidBulletStatusChanged(bool hasFire)
    {
        OnAcidBulletCountChanged?.Invoke(hasFire);
    }

    public static Action<bool> OnFireBulletStatusChanged;
    public static void RaiseFireBulletStatusChanged(bool hasFire)
    {
        OnFireBulletStatusChanged?.Invoke(hasFire);
    }

    public static void ResetItemUI()
    {
        OnShieldStatusChanged?.Invoke(false);
        OnHealthItemStatusChanged?.Invoke(false);
        OnAcidBulletCountChanged?.Invoke(false);
        OnFireBulletStatusChanged?.Invoke(false);
    }

}