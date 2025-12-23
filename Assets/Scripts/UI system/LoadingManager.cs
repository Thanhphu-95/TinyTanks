using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class LoadingManager : MonoBehaviour
{
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private Slider progressBar;
    [SerializeField] private TextMeshProUGUI progressText;


    private void OnEnable()
    {
        GameEvents.OnMapSelected += StartLoad;// Đăng ký sự kiện: "Cứ mỗi khi có Scene nào load xong thì báo cho tôi"
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        GameEvents.OnMapSelected -= StartLoad;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void StartLoad(string sceneName)
    {
        StartCoroutine(LoadAsync(sceneName));
    }

    // Hàm này sẽ tự động chạy NGAY SAU KHI qua scene mới
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Nếu là scene Menu thì không hiện Countdown
        if (scene.name == "Main Scene") return;

        Debug.Log("Đã vào Scene mới: " + scene.name + ". Đang tạo Countdown...");
        loadingPanel.SetActive(false);
    }

    IEnumerator LoadAsync(string sceneName)
    {
        loadingPanel.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // Ngăn Scene tự động kích hoạt để chúng ta kiểm soát thanh loading
        operation.allowSceneActivation = false;

        float targetProgress = 0f;

        // Vòng lặp cho đến khi thanh loading đầy 100%
        while (targetProgress < 1f)
        {
            // Lấy tiến trình thực tế từ Unity (tối đa là 0.9)
            float realProgress = Mathf.Clamp01(operation.progress / 0.9f);

            // Dùng Mathf.MoveTowards để thanh loading tăng dần dần, không bị nhảy vọt
            // 0.5f ở cuối là tốc độ tăng (bạn có thể chỉnh nhỏ lại nếu muốn load chậm hơn)
            targetProgress = Mathf.MoveTowards(targetProgress, realProgress, Time.deltaTime * 0.5f);

            if (progressBar != null) progressBar.value = targetProgress;
            if (progressText != null) progressText.text = (targetProgress * 100f).ToString("F0") + "%";

            // Khi thanh loading đã chạy đến 100% và Unity đã nạp xong ngầm
            if (targetProgress >= 1f && operation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.2f); // Đợi một chút cho người chơi kịp nhìn 100%
                operation.allowSceneActivation = true; // Chính thức cho phép vào Scene mới
            }

            yield return null;
        }

    }
}