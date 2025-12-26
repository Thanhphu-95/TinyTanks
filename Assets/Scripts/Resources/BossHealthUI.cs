using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthUI : MonoBehaviour
{
    [SerializeField] private Slider hpSlider;
    [SerializeField] private TextMeshProUGUI bossNameText;

    private void OnEnable()
    {
        // Lắng nghe sự kiện máu Boss thay đổi (Bạn cần thêm event này vào GameEvents)
        GameEvents.OnBossHealthChanged += UpdateBossUI;
    }

    private void OnDisable()
    {
        GameEvents.OnBossHealthChanged -= UpdateBossUI;
    }

    public void ShowBossBar(string name, int maxHp)
    {
        gameObject.SetActive(true);
        bossNameText.text = name;
        hpSlider.maxValue = maxHp;
        hpSlider.value = maxHp;
    }

    private void UpdateBossUI(int currentHp)
    {
        hpSlider.value = currentHp;
        if (currentHp <= 0) gameObject.SetActive(false);
    }
}