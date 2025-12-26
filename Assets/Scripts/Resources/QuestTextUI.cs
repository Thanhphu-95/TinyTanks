using UnityEngine;
using TMPro;

public class QuestTextUI : MonoBehaviour
{
    private TextMeshProUGUI questText;

    private void Awake()
    {
        questText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        // Đăng ký nhận sự kiện đổi chữ
        GameEvents.OnQuestTextChanged += UpdateQuestDisplay;
    }

    private void OnDisable()
    {
        GameEvents.OnQuestTextChanged -= UpdateQuestDisplay;
    }

    private void UpdateQuestDisplay(string newText)
    {
        if (questText != null)
        {
            questText.text = newText;
            // Bạn có thể thêm hiệu ứng nhấp nháy hoặc đổi màu tại đây khi có quest mới
            Debug.Log("UI nhận được tin nhắn: " + newText);
        }
    }
}