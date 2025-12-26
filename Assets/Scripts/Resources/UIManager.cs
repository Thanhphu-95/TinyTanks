using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using UnityEngine.UI; // Cần để điều khiển Slider
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    private GameObject currentPauseMenu;

    [Header("UI Prefabs")]
    private GameObject loadingPrefab;
    private GameObject countdownPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            loadingPrefab = Resources.Load<GameObject>("UI/LoadingCanvas");
            countdownPrefab = Resources.Load<GameObject>("UI/CountdownCanvas");
        }
        else { Destroy(gameObject); }
    }

    // QUAN TRỌNG: Update phải nằm ngoài Awake mới bắt được phím ESC
    private void Update()
    {
        // Nếu ở Main Scene thì không cho Pause
        if (SceneManager.GetActiveScene().name == "Main Scene") return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void ChangeScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        GameObject loadingScreen = null;
        Slider progressBar = null;
        TextMeshProUGUI progressText = null;

        if (loadingPrefab != null)
        {
            loadingScreen = Instantiate(loadingPrefab);
            DontDestroyOnLoad(loadingScreen);
            progressBar = loadingScreen.GetComponentInChildren<Slider>();
            progressText = loadingScreen.GetComponentInChildren<TextMeshProUGUI>();

            // Đảm bảo Loading luôn hiện trên cùng
            Canvas c = loadingScreen.GetComponent<Canvas>();
            if (c != null) { c.renderMode = RenderMode.ScreenSpaceOverlay; c.sortingOrder = 999; }
        }

        // Luôn đảm bảo thời gian chạy khi đang load
        Time.timeScale = 1f;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        float targetProgress = 0f;
        while (targetProgress < 1f)
        {
            // Tăng tốc độ targetProgress nhanh hơn một chút để người chơi không chờ lâu
            float realProgress = Mathf.Clamp01(operation.progress / 0.9f);
            targetProgress = Mathf.MoveTowards(targetProgress, realProgress, Time.unscaledDeltaTime * 0.5f); // Dùng unscaledDeltaTime để an toàn

            if (progressBar != null) progressBar.value = targetProgress;
            if (progressText != null) progressText.text = (targetProgress * 100f).ToString("F0") + "%";

            // Khi nạp xong ngầm
            if (operation.progress >= 0.9f && targetProgress >= 0.9f)
            {
                targetProgress = 1f; // Ép về 1
                if (progressBar != null) progressBar.value = 1f;
                if (progressText != null) progressText.text = "100%";

                yield return new WaitForSecondsRealtime(0.1f); // Đợi rất ngắn
                operation.allowSceneActivation = true; // Kích hoạt Scene mới ngay
            }
            yield return null;
        }

        // Chờ cho đến khi Scene thực sự đổi
        while (!operation.isDone) { yield return null; }

        // Xóa loading ngay lập tức khi vào Scene mới
        if (loadingScreen != null) Destroy(loadingScreen);
    }

    public void ShowCountdown(string missionContent, Action onFinished)
    {
        if (countdownPrefab != null)
        {
            GameObject go = Instantiate(countdownPrefab);
            var script = go.GetComponent<CountdownManager>();
            if (script != null) script.StartCountdown(missionContent, onFinished);
            else onFinished?.Invoke();
        }
    }

    public void TogglePauseMenu()
    {
        if (currentPauseMenu != null)
        {
            Time.timeScale = 1f;
            currentPauseMenu.GetComponent<PauseMenuManager>().CloseMenu();
            currentPauseMenu = null;
        }
        else
        {
            GameObject prefab = Resources.Load<GameObject>("UI/PauseMenuCanvas");
            if (prefab != null)
            {
                Time.timeScale = 0f;
                currentPauseMenu = Instantiate(prefab);
                currentPauseMenu.GetComponent<PauseMenuManager>().OpenMenu();
            }
        }
    }
}