using TMPro;
using UnityEngine;

public class QuestTextUI : MonoBehaviour
{
    public static QuestTextUI Instance;

    public TMP_Text messageText;
    public float messageDuration = 3f;

    private float timer;
    private bool showing = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        Hide();
    }

    void Update()
    {
        if (!showing) return;

        timer -= Time.deltaTime;
        if (timer <= 0)
            Hide();
    }

    public void Show(string msg)
    {
        if (messageText == null)
        {
            Debug.LogWarning("Chưa gán TMP_Text cho QuestMessageDisplay");
            return;
        }

        messageText.text = msg;
        messageText.gameObject.SetActive(true);
        timer = messageDuration;
        showing = true;
    }

    public void Hide()
    {
        messageText.gameObject.SetActive(false);
        showing = false;
    }
}
