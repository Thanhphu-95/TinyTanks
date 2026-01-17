using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    private GameObject optionsInstance; // Lưu đối tượng đã tạo ra để quản lý
    public GameObject gameplayRoot;

    public void OpenMenu()
    {
        if (mainPanel == null) return;
        mainPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CloseMenu()
    {
        Time.timeScale = 1f;
        // Nếu đang mở options thì xóa nó luôn
        if (optionsInstance != null) Destroy(optionsInstance);
        Destroy(gameObject);
    }

    public void RestartGame()
    {
        GameEvents.ResetItemUI();
        string currentScene = SceneManager.GetActiveScene().name;
        UIManager.Instance.ChangeScene(currentScene);
        AudioListener.volume = 0f;

    }

    public void ShowOptions()
    {
        // 1. Chỉ Load và tạo nếu nó chưa tồn tại
        if (optionsInstance == null)
        {
            // Load prefab của bảng Setting (đã làm ở các bước trước)
            GameObject prefab = Resources.Load<GameObject>("UI/Setting");
            if (prefab != null)
            {
                optionsInstance = Instantiate(prefab);
            }
        }
        Time.timeScale = 0f;
        // 2. Ẩn menu chính và hiện bảng setting
        if (optionsInstance != null)
        {
            mainPanel.SetActive(false);
            optionsInstance.SetActive(true);

            // Đảm bảo nút Back trong bảng Setting có thể tìm lại Menu Pause này
            // Bạn có thể dùng Event hoặc tìm script AudioSettingsController để gán sự kiện
        }
    }

    public void BackToMainPause()
    {
        mainPanel.SetActive(true);
        // Nếu dùng Destroy bảng setting khi đóng thì ở đây không cần làm gì thêm
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        UIManager.Instance.ChangeScene("Main Scene");
        Debug.Log("đang quay lại Main Scene");
        AudioListener.volume = 0f;
    }
}