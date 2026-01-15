using TMPro;                                      // TextMeshPro
using UnityEngine;                               // Unity core
using UnityEngine.SceneManagement;               // Quản lý Scene
using UnityEngine.UI;                            // UI (Slider)

public class InGameUIManager : MonoBehaviour      // Quản lý UI trong gameplay
{
    public static InGameUIManager Instance { get; private set; } // Singleton

    private GameObject hudInstance;               // Instance HUD
    private Slider healthSlider;                  // Thanh máu
    private TextMeshProUGUI hpText;               // Text HP
    private TextMeshProUGUI timerText;            // Text thời gian
    private MissionResultUI missionResult;        // UI kết quả nhiệm vụ

    private float targetHP;                       // HP mục tiêu để lerp


    private GameObject bossUIGroup;     // Group chứa Slider và Text của Boss
    private Slider bossHealthSlider;    // Slider máu Boss
    private TextMeshProUGUI bossNameText;
    private void Awake()                          // Gọi khi object được tạo
    {
        if (Instance == null)                     // Nếu chưa có Instance
        {
            Instance = this;                      // Gán instance
            DontDestroyOnLoad(gameObject);        // Không hủy khi đổi scene
            LoadUI();                             // Load HUD
            SceneManager.sceneLoaded += OnSceneLoaded; // Lắng nghe sự kiện load scene
        }
        else { Destroy(gameObject); }             // Tránh trùng manager
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) // Khi scene load xong
    {
        if (scene.name == "Main Scene")           // Nếu là menu
        {
            ShowHUD(false);                       // Ẩn toàn bộ UI
        }
        else                                      // Scene gameplay
        {
            PrepareUIForNewGame();                // Chuẩn bị UI cho lượt chơi mới
        }
    }

    private void LoadUI()                         // Load prefab HUD
    {
        GameObject prefab = Resources.Load<GameObject>("UI/InGame_HUD"); // Load prefab
        if (prefab == null)                       // Không tìm thấy
        {
            Debug.LogError("KHÔNG TÌM THẤY PREFAB TẠI: Resources/UI/InGame_HUD"); // Báo lỗi
            return;                               // Thoát
        }

        hudInstance = Instantiate(prefab, transform); // Tạo HUD
        hudInstance.SetActive(false);             // Ẩn HUD ban đầu
        healthSlider = hudInstance.GetComponentInChildren<Slider>(); // Lấy slider máu

        hpText = FindInChild<TextMeshProUGUI>("HP_Text"); // Tìm text HP
        timerText = FindInChild<TextMeshProUGUI>("Timer_Text"); // Tìm text timer
        missionResult = hudInstance.GetComponentInChildren<MissionResultUI>(true); // Lấy UI kết quả

        if (missionResult == null)                // Nếu không có MissionResultUI
        {
            Debug.LogError("CẢNH BÁO: Không tìm thấy script MissionResultUI trong Prefab HUD!"); // Cảnh báo
        }

        PrepareUIForNewGame();                    // Khởi tạo trạng thái UI

        Transform bUI = hudInstance.transform.Find("InGame/BossUI");
        if (bUI != null)
        {
            bossUIGroup = bUI.gameObject;
            bossHealthSlider = bUI.GetComponentInChildren<Slider>();
            bossNameText = bUI.Find("Boss_Name_Text")?.GetComponent<TextMeshProUGUI>();
            bossUIGroup.SetActive(false); // Luôn ẩn khi bắt đầu
        }
    }

    // ĐÂY LÀ HÀM QUAN TRỌNG NHẤT ĐỂ SỬA LỖI RESTART
    public void ResetUI()                         // Reset UI khi restart
    {
        if (hudInstance == null) return;          // Chưa load HUD thì bỏ

        Time.timeScale = 1f;                      // Đảm bảo game đang chạy

        Transform inGame = hudInstance.transform.Find("InGame"); // Tìm group InGame
        if (inGame) inGame.gameObject.SetActive(true); // Hiện UI InGame

        if (missionResult != null) missionResult.gameObject.SetActive(false); // Ẩn EndGame

        hudInstance.SetActive(true);               // Bật HUD tổng
    }

    private void OnEnable()                        // Khi script được enable
    {
        GameEvents.OnPlayerHealthChanged += HandleHealthChanged; // Subscribe HP
        GameEvents.OnQuestTimeUpdate += HandleTimeUpdate;        // Subscribe timer
    }

    private void HandleHealthChanged(int curr, int max) // Khi HP thay đổi
    {
        targetHP = curr;                          // Set HP mục tiêu
        if (healthSlider) healthSlider.maxValue = max; // Set max HP
        if (hpText) hpText.text = $"{curr}/{max}";     // Cập nhật text
    }

    private void HandleTimeUpdate(float time)     // Khi timer cập nhật
    {
        if (!timerText) return;                   // Không có text thì bỏ
        int m = Mathf.FloorToInt(time / 60);      // Tính phút
        int s = Mathf.FloorToInt(time % 60);      // Tính giây
        timerText.text = string.Format("{0:00}:{1:00}", m, s); // Format mm:ss
    }

    public void ShowHUD(bool status)               // Bật / tắt HUD
    {
        if (hudInstance) hudInstance.SetActive(status); // Set active HUD
    }

    public void ShowEndGame(bool isWin)            // Hiện bảng kết quả
    {
        if (missionResult) missionResult.Show(isWin); // Show Win / Lose

        Transform inGame = hudInstance.transform.Find("InGame"); // Tìm group InGame
        if (inGame) inGame.gameObject.SetActive(false); // Ẩn UI InGame
    }

    private void Update()                          // Update mỗi frame
    {
        if (healthSlider)                          // Nếu có slider
            healthSlider.value = Mathf.Lerp(healthSlider.value, targetHP, Time.deltaTime * 10f); // Lerp HP
    }

    private T FindInChild<T>(string name) where T : Component // Tìm component theo tên
    {
        if (hudInstance == null) return null;      // Chưa có HUD
        T[] comps = hudInstance.GetComponentsInChildren<T>(true); // Lấy toàn bộ component
        foreach (var c in comps)                   // Duyệt từng component
            if (c.gameObject.name == name) return c; // Trả về nếu trùng tên
        return null;                               // Không tìm thấy
    }

    private void OnDisable()                       // Khi object bị hủy
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe scene
        GameEvents.OnPlayerHealthChanged -= HandleHealthChanged; // Unsubscribe HP
        GameEvents.OnQuestTimeUpdate -= HandleTimeUpdate;        // Unsubscribe timer
    }

    public void PrepareUIForNewGame()              // Chuẩn bị UI cho game mới
    {
        if (hudInstance == null) return;           // Chưa load HUD

        Time.timeScale = 1f;                       // Đảm bảo game không pause

        if (missionResult != null) missionResult.gameObject.SetActive(false); // Ẩn EndGame

        Transform inGame = hudInstance.transform.Find("InGame"); // Tìm group InGame
        if (inGame) inGame.gameObject.SetActive(true); // Bật UI InGame

        hudInstance.SetActive(false);               // Giữ HUD đang ẩn
    }

    public void InitBossHealthBar(string name, float startPercent)
    {
        if (bossUIGroup == null) return;

        bossUIGroup.SetActive(true);
        if (bossNameText) bossNameText.text = name;
        if (bossHealthSlider) bossHealthSlider.value = startPercent;
    }

    // Hàm cập nhật giá trị máu Boss
    public void UpdateBossHealth(float percent)
    {
        if (bossHealthSlider)
        {
            // Bạn có thể gán trực tiếp hoặc dùng Lerp giống thanh máu Player
            bossHealthSlider.value = percent;
        }
    }

    // Hàm ẩn thanh máu khi Boss chết
    public void HideBossHealthBar()
    {
        if (bossUIGroup) bossUIGroup.SetActive(false);
    }
}
