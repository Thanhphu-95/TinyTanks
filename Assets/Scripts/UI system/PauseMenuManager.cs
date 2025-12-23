using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject optionsPanel;

    public void OpenMenu()
    {
        if (mainPanel == null || optionsPanel == null) return;

        mainPanel.SetActive(true);
        optionsPanel.SetActive(false);
        Time.timeScale = 0f;
    }

    public void CloseMenu()
    {
        Time.timeScale = 1f;
        Destroy(gameObject);
    }

    // --- HÀM RESTART MỚI THÊM ---
    public void RestartGame()
    {
        Time.timeScale = 1f;


        string currentScene = SceneManager.GetActiveScene().name;
        UIManager.Instance.ChangeScene(currentScene);
    }

    public void ShowOptions()
    {
        mainPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void BackToMainPause()
    {
        optionsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        UIManager.Instance.ChangeScene("Main Scene");
        Debug.Log("đang quay lại Main Scene");
    }
}