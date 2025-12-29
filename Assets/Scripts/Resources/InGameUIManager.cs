using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    public static InGameUIManager Instance { get; private set; }

    private GameObject hudInstance;
    private Slider healthSlider;
    private TextMeshProUGUI hpText;
    private TextMeshProUGUI timerText;
    private MissionResultUI missionResult;

    private float targetHP;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadUI();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else { Destroy(gameObject); }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main Scene")
        {
            ShowHUD(false); // Ẩn toàn bộ UI khi ở Menu
        }
        else
        {
            // Khi load scene chơi game (hoặc Restart)
            PrepareUIForNewGame();
        }
    }

    private void LoadUI()
    {
        GameObject prefab = Resources.Load<GameObject>("UI/InGame_HUD");
        if (prefab == null)
        {
            Debug.LogError("KHÔNG TÌM THẤY PREFAB TẠI: Resources/UI/InGame_HUD");
            return;
        }

        hudInstance = Instantiate(prefab, transform);
        hudInstance.SetActive(false);
        healthSlider = hudInstance.GetComponentInChildren<Slider>();

        hpText = FindInChild<TextMeshProUGUI>("HP_Text");
        timerText = FindInChild<TextMeshProUGUI>("Timer_Text");
        missionResult = hudInstance.GetComponentInChildren<MissionResultUI>(true);

        if (missionResult == null)
        {
            Debug.LogError("CẢNH BÁO: Không tìm thấy script MissionResultUI trong Prefab HUD!");
        }

        // Khởi tạo trạng thái ban đầu
        PrepareUIForNewGame();
    }

    // ĐÂY LÀ HÀM QUAN TRỌNG NHẤT ĐỂ SỬA LỖI RESTART CỦA BẠN
    public void ResetUI()
    {
        if (hudInstance == null) return;

        // Đảm bảo thời gian chạy bình thường
        Time.timeScale = 1f;


        Transform inGame = hudInstance.transform.Find("InGame");
        if (inGame) inGame.gameObject.SetActive(true);

        // 2. Ẩn bảng EndGame đi
        if (missionResult != null) missionResult.gameObject.SetActive(false);

        // Kích hoạt HUD tổng
        hudInstance.SetActive(true);
    }

    private void OnEnable()
    {
        GameEvents.OnPlayerHealthChanged += HandleHealthChanged;
        GameEvents.OnQuestTimeUpdate += HandleTimeUpdate;
    }

    // Tách ra hàm để dễ quản lý và tránh lỗi lambda khi Unsubscribe
    private void HandleHealthChanged(int curr, int max)
    {
        targetHP = curr;
        if (healthSlider) healthSlider.maxValue = max;
        if (hpText) hpText.text = $"{curr}/{max}";
    }

    private void HandleTimeUpdate(float time)
    {
        if (!timerText) return;
        int m = Mathf.FloorToInt(time / 60);
        int s = Mathf.FloorToInt(time % 60);
        timerText.text = string.Format("{0:00}:{1:00}", m, s);
    }

    public void ShowHUD(bool status) { if (hudInstance) hudInstance.SetActive(status); }

    public void ShowEndGame(bool isWin)
    {
        if (missionResult) missionResult.Show(isWin);

        // Ẩn nhóm InGame khi bảng EndGame hiện lên
        Transform inGame = hudInstance.transform.Find("InGame");
        if (inGame) inGame.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (healthSlider) healthSlider.value = Mathf.Lerp(healthSlider.value, targetHP, Time.deltaTime * 10f);
    }

    private T FindInChild<T>(string name) where T : Component
    {
        if (hudInstance == null) return null;
        T[] comps = hudInstance.GetComponentsInChildren<T>(true);
        foreach (var c in comps) if (c.gameObject.name == name) return c;
        return null;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        GameEvents.OnPlayerHealthChanged -= HandleHealthChanged;
        GameEvents.OnQuestTimeUpdate -= HandleTimeUpdate;
    }

    public void PrepareUIForNewGame()
    {
        if (hudInstance == null) return;

        Time.timeScale = 1f;

        // Ẩn bảng kết quả
        if (missionResult != null) missionResult.gameObject.SetActive(false);

        // Bật group InGame bên trong nhưng vẫn giữ hudInstance là FALSE
        Transform inGame = hudInstance.transform.Find("InGame");
        if (inGame) inGame.gameObject.SetActive(true);

        hudInstance.SetActive(false); // Đảm bảo nó vẫn đang ẩn
    }
}