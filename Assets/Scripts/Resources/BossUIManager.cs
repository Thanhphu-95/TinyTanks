using UnityEngine;
using UnityEngine.UI;

public class BossUIManager : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;

    private void Awake()
    {
        // Tự động tìm Slider trong chính nó hoặc các con của nó
        healthSlider = GetComponentInChildren<Slider>();

        if (healthSlider == null)
        {
            Debug.LogError("LỖI: Không tìm thấy Slider nào trong Prefab Boss_Health_Canvas!");
        }
    }

    public void Setup(float percent)
    {
        if (healthSlider != null) healthSlider.value = percent;
    }

    public void UpdateHealth(float percent)
    {
        if (healthSlider != null) healthSlider.value = percent;
    }

    public void Close()
    {
        Destroy(gameObject);
    }
}