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
            Debug.Log("tìm thấy loading");
            loadingScreen = Instantiate(loadingPrefab);
            DontDestroyOnLoad(loadingScreen);
            // Tìm Slider và Text trong Loading Screen
            progressBar = loadingScreen.GetComponentInChildren<Slider>();
            progressText = loadingScreen.GetComponentInChildren<TextMeshProUGUI>();
        }

        Time.timeScale = 1f;
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false; // Chặn để hiện loading cho đẹp

        float targetProgress = 0f;
        while (targetProgress < 1f)
        {
            float realProgress = Mathf.Clamp01(operation.progress / 0.9f);
            targetProgress = Mathf.MoveTowards(targetProgress, realProgress, Time.deltaTime * 0.1f);

            if (progressBar != null) progressBar.value = targetProgress;
            if (progressText != null) progressText.text = (targetProgress * 100f).ToString("F0") + "%";

            if (targetProgress >= 1f && operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);
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
            currentPauseMenu.GetComponent<PauseMenuManager>().CloseMenu();
            currentPauseMenu = null;
        }
        else
        {
            GameObject prefab = Resources.Load<GameObject>("UI/PauseMenuCanvas");
            if (prefab != null)
            {
                currentPauseMenu = Instantiate(prefab);
                currentPauseMenu.GetComponent<PauseMenuManager>().OpenMenu();
            }
        }
    }
}