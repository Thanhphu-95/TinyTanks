using UnityEngine;                          // Thư viện Unity cơ bản
using UnityEngine.SceneManagement;          // Quản lý Scene
using System.Collections;                   // Dùng cho Coroutine
using System;                               // Dùng Action
using UnityEngine.UI;                       // Điều khiển UI (Slider)
using TMPro;                                // TextMeshPro

public class UIManager : MonoBehaviour      // Class quản lý toàn bộ UI
{
    public static UIManager Instance { get; private set; }   // Singleton
    private GameObject currentPauseMenu;                     // Menu pause hiện tại

    [Header("UI Prefabs")]
    private GameObject loadingPrefab;        // Prefab loading screen
    private GameObject countdownPrefab;      // Prefab countdown
    public bool isCountdownActive = false;

    private void Awake()                     // Chạy khi object được tạo
    {
        if (Instance == null)                // Nếu chưa có Instance
        {
            Instance = this;                 // Gán instance
            DontDestroyOnLoad(gameObject);   // Không bị hủy khi đổi scene
            loadingPrefab = Resources.Load<GameObject>("UI/LoadingCanvas");     // Load loading UI
            countdownPrefab = Resources.Load<GameObject>("UI/CountdownCanvas"); // Load countdown UI
        }
        else
        {
            Destroy(gameObject);             // Tránh tạo trùng UIManager
        }
    }

    // QUAN TRỌNG: Update phải nằm ngoài Awake mới bắt được phím ESC
    private void Update()                    // Chạy mỗi frame
    {
        if (SceneManager.GetActiveScene().name == "Main Scene") return; // Không pause ở Main Scene

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isCountdownActive) return; // ⛔ đang countdown thì không pause
            TogglePauseMenu();
        }
    }

    public void ChangeScene(string sceneName) // Hàm đổi scene
    {
        StartCoroutine(LoadSceneAsync(sceneName)); // Load scene bất đồng bộ
    }

    private IEnumerator LoadSceneAsync(string sceneName) // Coroutine load scene
    {
        GameObject loadingScreen = null;     // UI loading
        Slider progressBar = null;            // Thanh tiến trình
        TextMeshProUGUI progressText = null;  // Text %

        if (loadingPrefab != null)            // Nếu có prefab loading
        {
            loadingScreen = Instantiate(loadingPrefab); // Tạo loading UI
            DontDestroyOnLoad(loadingScreen); // Không bị destroy khi đổi scene
            progressBar = loadingScreen.GetComponentInChildren<Slider>(); // Lấy slider
            progressText = loadingScreen.GetComponentInChildren<TextMeshProUGUI>(); // Lấy text %

            Canvas c = loadingScreen.GetComponent<Canvas>(); // Lấy canvas
            if (c != null)
            {
                c.renderMode = RenderMode.ScreenSpaceOverlay; // Hiển thị overlay 
                c.sortingOrder = 999;                         // Luôn trên cùng
            }
        }

        Time.timeScale = 1f;                  // Đảm bảo game không bị pause khi load

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName); // Load scene
        operation.allowSceneActivation = false; // Chưa cho vào scene mới

        float targetProgress = 0f;            // Giá trị hiển thị giả
        while (targetProgress < 1f)           // Khi chưa load xong
        {
            float realProgress = Mathf.Clamp01(operation.progress / 0.9f); // Chuẩn hóa progress
            targetProgress = Mathf.MoveTowards(
                targetProgress,
                realProgress,
                Time.unscaledDeltaTime * 0.1f); // Tăng mượt, không phụ thuộc timescale

            if (progressBar != null)
                progressBar.value = targetProgress; // Cập nhật slider

            if (progressText != null)
                progressText.text = (targetProgress * 100f).ToString("F0") + "%"; // Cập nhật %

            if (operation.progress >= 0.9f && targetProgress >= 0.9f) // Load xong ngầm
            {
                targetProgress = 1f;           // Ép full
                if (progressBar != null)
                    progressBar.value = 1f;    // Slider 100%

                if (progressText != null)
                    progressText.text = "100%"; // Text 100%

                yield return new WaitForSecondsRealtime(0.5f); // thời gian đợi
                operation.allowSceneActivation = true; // Cho phép vào scene mới
            }
            yield return null;                 // Chờ frame tiếp theo
        }

        while (!operation.isDone)              // Đợi scene load hoàn toàn
        {
            yield return null;
        }

        if (loadingScreen != null)             // Nếu còn loading UI
            Destroy(loadingScreen);            // Hủy loading
        AudioListener.volume = 1f;

    }

    public void ShowCountdown(string missionContent, Action onFinished)
    {
        if (countdownPrefab != null)
        {
            isCountdownActive = true; // ⛔ KHÓA PAUSE

            GameObject go = Instantiate(countdownPrefab);
            var script = go.GetComponent<CountdownManager>();

            if (script != null)
            {
                script.StartCountdown(missionContent, () =>
                {
                    isCountdownActive = false; // ✅ MỞ LẠI PAUSE
                    onFinished?.Invoke();
                });
            }
            else
            {
                isCountdownActive = false;
                onFinished?.Invoke();
            }
        }
    }


    public void TogglePauseMenu()               // Bật / tắt pause
    {
        if (currentPauseMenu != null)           // Nếu đang pause
        {
            Time.timeScale = 1f;                // Resume game
            currentPauseMenu.GetComponent<PauseMenuManager>().CloseMenu(); // Đóng UI
            currentPauseMenu = null;            // Reset reference
        }
        else                                    // Nếu chưa pause
        {
            GameObject prefab = Resources.Load<GameObject>("UI/PauseMenuCanvas"); // Load prefab
            if (prefab != null)                 // Nếu tồn tại
            {
                Time.timeScale = 0f;            // Dừng game
                currentPauseMenu = Instantiate(prefab); // Tạo menu
                currentPauseMenu.GetComponent<PauseMenuManager>().OpenMenu(); // Mở UI
            }
        }
    }
}
