using UnityEngine;
using TMPro;

public class QuestTextUI : MonoBehaviour
{
    public TMP_Text questText;

    public void UpdateQuest(string text)
    {
        questText.text = text;
    }
}
