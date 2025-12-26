using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // Thêm namespace này để đổi chữ nếu cần

public class MissionResultUI : MonoBehaviour
{
    [Header("Nút bấm")]
    [SerializeField] private Button restartButton;
    [SerializeField] private Button exitButton;

    [Header("Giao diện (Tùy chọn)")]
    [SerializeField] private TextMeshProUGUI titleText; // Để hiện "VICTORY" hoặc "DEFEAT"
    [SerializeField] private Image background;      // Để đổi màu nền nếu muốn

    [Header("Cấu hình")]
    [SerializeField] private string menuSceneName = "Main Scene";

    void Awake()
    {
        if (restartButton != null) restartButton.onClick.AddListener(RestartGame);
        if (exitButton != null) exitButton.onClick.AddListener(ExitToMenu);
    }

    // Cập nhật hàm Show có thêm tham số isWin
    public void Show(bool isWin)
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;

        // Tự động điều chỉnh giao diện dựa trên kết quả
        if (titleText != null)
        {
            titleText.text = isWin ? "Mission accomplie!" : "Mission Failed !";
            //titleText.color = isWin ? Color.white : Color.white;
        }

        if (background != null)
        {
            // Thắng thì nền xanh nhẹ, thua thì nền đỏ nhẹ (tùy bạn chỉnh)
            background.color = isWin ? new Color(0, 1, 0, 0.3f) : new Color(1, 0, 0, 0.3f);
        }
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ExitToMenu()
    {
        Time.timeScale = 1f;
        if (InGameUIManager.Instance != null) Destroy(InGameUIManager.Instance.gameObject);
        SceneManager.LoadScene("Main Scene");
    }
}