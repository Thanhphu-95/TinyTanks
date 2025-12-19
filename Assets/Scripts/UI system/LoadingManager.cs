using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class LoadingManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject loadingPanel;    // Panel chứa toàn bộ UI loading
    [SerializeField] private Slider progressBar;         // Thanh chạy
    [SerializeField] private TextMeshProUGUI progressText; // Chữ hiện %

    private void Awake()
    {
        // Đảm bảo khi mới vào Game, màn hình loading phải ẩn đi
        loadingPanel.SetActive(false);

        // GIỮ CHO LOADING KHÔNG BỊ XÓA (Tùy chọn)
        // Nếu bạn muốn dùng 1 Loading duy nhất xuyên suốt game:
        // DontDestroyOnLoad(gameObject); 
    }

    private void OnEnable()
    {
        // Đăng ký nghe sự kiện chọn Map từ trạm phát GameEvents
        GameEvents.OnMapSelected += StartLoadingProcess;
    }

    private void OnDisable()
    {
        // Hủy đăng ký khi object bị ẩn/xóa để tránh lỗi
        GameEvents.OnMapSelected -= StartLoadingProcess;
    }

    private void StartLoadingProcess(string sceneName)
    {
        // Bắt đầu tiến trình nạp bất đồng bộ
        StartCoroutine(LoadAsync(sceneName));
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
            targetProgress = Mathf.MoveTowards(targetProgress, realProgress, Time.deltaTime * 0.2f);

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