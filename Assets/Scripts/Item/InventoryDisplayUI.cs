using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplayUI : MonoBehaviour
{
    [Header("Cấu hình 5 ô UI")]
    // Kéo các Image (Icon) của ô 1, 2, 3, 4 vào đây theo đúng thứ tự
    public GameObject iconShield; // Ô 1
    public GameObject iconHealth; // Ô 2
    public GameObject iconAcid;   // Ô 3
    public GameObject iconFire;   // Ô 4

    private ItemManager itemManager;

    void Start()
    {
        // Tìm ItemManager trên xe (Player)
        itemManager = GameObject.FindGameObjectWithTag("Player").GetComponent<ItemManager>();

        // Mặc định ẩn hết Icon khi mới vào game
        if (iconShield) iconShield.SetActive(false);
        if (iconHealth) iconHealth.SetActive(false);
        if (iconAcid) iconAcid.SetActive(false);
        if (iconFire) iconFire.SetActive(false);
    }
    private void OnEnable()
    {
        // Đăng ký lắng nghe: "Khi nào có sự kiện Shield thay đổi, hãy gọi hàm UpdateShieldUI"
        GameEvents.OnShieldStatusChanged += UpdateShieldUI;
        GameEvents.OnHealthItemStatusChanged += UpdateHealthUI;
        GameEvents.OnAcidBulletCountChanged += UpdateAcidUI;
        GameEvents.OnFireBulletStatusChanged += UpdateFireUI;
    }
    private void OnDisable()
    {
        // Hủy đăng ký để tránh lỗi bộ nhớ
        GameEvents.OnShieldStatusChanged -= UpdateShieldUI;
        GameEvents.OnHealthItemStatusChanged -= UpdateHealthUI;
        GameEvents.OnAcidBulletCountChanged -= UpdateAcidUI;
        GameEvents.OnFireBulletStatusChanged -= UpdateFireUI;

    }
    private void UpdateShieldUI(bool hasShield)
    {
        if (iconShield != null)
        {
            iconShield.SetActive(hasShield);
            Debug.Log("UI: Cập nhật trạng thái khiên -> " + hasShield);
        }
    }
    private void UpdateHealthUI(bool hasHealth)
    {
        if (iconHealth != null)
        {
            iconHealth.SetActive(hasHealth);
        }
    }
    private void UpdateAcidUI(bool hasAcid)
    {
        if (iconAcid != null) iconAcid.SetActive(hasAcid);
    }
    private void UpdateFireUI(bool hasFire)
    {
        if (iconFire != null) iconFire.SetActive(hasFire);
    }
}