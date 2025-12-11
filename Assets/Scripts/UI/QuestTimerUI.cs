using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestTimerUI : MonoBehaviour
{
    public Image circleFill;
    public TMP_Text timeText;

    public void UpdateTimer(float current, float max)
    {
        // Fill vòng tròn
        circleFill.fillAmount = current / max;

        // Hiển thị số
        int minutes = Mathf.FloorToInt(current / 60);
        int seconds = Mathf.FloorToInt(current % 60);

        timeText.text = $"{minutes:00}:{seconds:00}";
    }
}
