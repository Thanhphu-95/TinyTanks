using UnityEngine;
using TMPro;
using System.Collections;
using System;

public class CountdownManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private TextMeshProUGUI missionText;
    [SerializeField] private GameObject countdownPanel;

    [Header("Settings")]
    [SerializeField] private int countdownTime = 3;

    public void StartCountdown(string missionContent, Action onFinished)
    {
        if (missionText != null) missionText.text = missionContent;
        StartCoroutine(HandleCountdown(onFinished));
    }

    private IEnumerator HandleCountdown(Action onFinished)
    {
        Time.timeScale = 0; // Dừng game để đếm ngược
        if (countdownPanel != null) countdownPanel.SetActive(true);

        int current = countdownTime;
        while (current > 0)
        {
            if (countdownText != null) countdownText.text = current.ToString();
            yield return new WaitForSecondsRealtime(1f);
            current--;
        }

        if (countdownText != null) countdownText.text = "BẮT ĐẦU!";
        yield return new WaitForSecondsRealtime(0.5f);

        Time.timeScale = 1; // Mở lại game
        onFinished?.Invoke();

        Destroy(gameObject);
    }
}