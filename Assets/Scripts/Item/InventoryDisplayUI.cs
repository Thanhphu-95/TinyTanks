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

    void Update()
    {
        if (itemManager == null) return;

        // Cập nhật trạng thái hiển thị dựa trên biến bool trong ItemManager
        // Sử dụng toán tử điều kiện để bật/tắt Active
        if (iconShield) iconShield.SetActive(itemManager.CanUseShield());
        if (iconHealth) iconHealth.SetActive(itemManager.CanUseHealth());
        if (iconAcid) iconAcid.SetActive(itemManager.CanUseAcid());
        if (iconFire) iconFire.SetActive(itemManager.CanUseFire());
    }
}