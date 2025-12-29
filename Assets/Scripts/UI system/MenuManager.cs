using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;                        // Kéo Panel Menu chính vào đây
    public GameObject mapSelectionPanel;                    // Kéo Panel Chọn Map vào đây
    private GameObject optionsPrefab;

    private void OnEnable()// Khi Object này được kích hoạt
    {
        GameEvents.OnMapSelected += HandleMapSelected;      // Đăng ký: "Khi có map được chọn, gọi tôi"
    }
    
    private void OnDisable()// Khi Object này bị tắt hoặc hủy
    {
        GameEvents.OnMapSelected -= HandleMapSelected;      // Hủy đăng ký để tránh lỗi bộ nhớ
    }

    // Hàm xử lý khi nhận được tín hiệu chọn Map
    private void HandleMapSelected(string sceneName)
    {
        Debug.Log("Đang chuyển cảnh sang: " + sceneName);    // Log để kiểm tra trong Console
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ChangeScene(sceneName);
        }
        else
        {
            // Trường hợp chạy test map lẻ không qua Main Scene
            SceneManager.LoadScene(sceneName);
        }
    }

    // --- CÁC HÀM ĐIỀU KHIỂN PANEL (Gắn trực tiếp vào nút Start/Back) ---

    public void ShowMapSelection()                          // Gọi khi bấm nút "Start Game"
    {
        mainMenuPanel.SetActive(false);                     // Ẩn menu chính
        mapSelectionPanel.SetActive(true);                  // Hiện menu chọn map
    }

    public void ShowMainMenu()                               // Gọi khi bấm nút "Back"
    {
        mainMenuPanel.SetActive(true);                      // Hiện menu chính
        mapSelectionPanel.SetActive(false);                 // Ẩn menu chọn map
    }
    public void ShowOptions()
    {
        Debug.Log("ko tìm thấy setting");
        // Bật Canvas Setting lên
        if (optionsPrefab == null)
        {
            GameObject prefab = Resources.Load<GameObject>("UI/Setting");
            Debug.Log("bật setting");
            optionsPrefab = Instantiate(prefab);
            

        }
    }
    public void QuitGame()                                  // Gọi khi bấm nút "Exit"
    {
        Debug.Log("Thoát game");
        Application.Quit();                                 // Thoát ứng dụng
    }
}