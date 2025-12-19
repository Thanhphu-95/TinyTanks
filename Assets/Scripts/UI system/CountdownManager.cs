using UnityEngine;
using TMPro;
using System.Collections;

public class CountdownManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI countdownText;   // Text hiện số 3, 2, 1
    [SerializeField] private TextMeshProUGUI missionText;     // Text hiện nội dung nhiệm vụ
    [SerializeField] private GameObject countdownPanel;      // Panel tổng để ẩn khi xong

    [Header("Mission Settings")]
    [TextArea(3, 10)] // Cho phép gõ nhiều dòng trong Inspector
    [SerializeField] private string missionContent;          // Nội dung nhiệm vụ riêng cho từng Map
    [SerializeField] private int countdownTime = 5;           // Thời gian chờ

    private void Start()
    {
        // Gán nội dung nhiệm vụ đã gõ từ Inspector vào UI
        if (missionText != null) missionText.text = missionContent;

        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        Time.timeScale = 0; // Tạm dừng mọi thứ trong game (Physics, AI...)

        int currentTime = countdownTime;

        while (currentTime > 0)
        {
            if (countdownText != null) countdownText.text = currentTime.ToString();

            // Dùng WaitForSecondsRealtime vì Time.timeScale đang bằng 0
            yield return new WaitForSecondsRealtime(1f);

            currentTime--;
        }

        if (countdownText != null) countdownText.text = "BẮT ĐẦU!";
        yield return new WaitForSecondsRealtime(0.5f);

        Time.timeScale = 1; // Mở lại thời gian cho game chạy

        if (countdownPanel != null) countdownPanel.SetActive(false); // Ẩn toàn bộ UI đếm ngược
    }
}