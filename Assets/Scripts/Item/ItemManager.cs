// =======================
// QUẢN LÝ ITEM CHO PLAYER
// =======================
using UnityEngine;

// Script quản lý toàn bộ item người chơi có thể nhặt và sử dụng
public class ItemManager : MonoBehaviour
{
    // ===== KHIÊN Ô (SHIELD) =====
    [Header("UmbrellaShield")]
    public GameObject umbrellaShieldPrefab; // Prefab khiên ô
    public Transform attachPoint;           // Điểm gắn khiên (nòng súng / turret)
    public float shieldDuration = 3f;       // Thời gian tồn tại của khiên
    private bool canUseShield = false;      // Đã nhặt khiên hay chưa

    // ===== HỒI MÁU =====
    [Header("health")]
    public GameObject healthEff;             // Hiệu ứng hồi máu
    public int totalHealAmount = 200;         // Tổng lượng máu hồi
    public float duration = 3f;              // Thời gian hồi máu
    private bool canUseHealth = false;       // Có thể dùng item hồi máu hay không

    // ===== ĐẠN ACID =====
    [Header("AxitBullet")]
    public GameObject acidBulletPrefab;      // Prefab đạn acid
    private bool canUseAcid = false;          // Có thể dùng đạn acid hay không

    // ===== ĐẠN LỬA =====
    [Header("FireBullet")]
    public GameObject fireBulletPrefab;      // Prefab đạn lửa
    private bool canUsefire = false;          // Có thể dùng đạn lửa hay không

    // ===== HÀM KIỂM TRA TRẠNG THÁI ITEM (CHO UI / SCRIPT KHÁC) =====
    public bool CanUseShield() => canUseShield; // Có khiên hay không
    public bool CanUseHealth() => canUseHealth; // Có máu hay không
    public bool CanUseAcid() => canUseAcid;     // Có đạn acid hay không
    public bool CanUseFire() => canUsefire;     // Có đạn lửa hay không

    // Update chạy mỗi frame
    void Update()
    {
        // BẤM PHÍM 1 → DÙNG KHIÊN
        if (Input.GetKeyDown(KeyCode.Alpha1) && canUseShield)
        {
            UseUmbrella();
        }

        // BẤM PHÍM 2 → DÙNG HỒI MÁU
        if (Input.GetKeyDown(KeyCode.Alpha2) && canUseHealth)
        {
            UseHealth();
        }

        // BẤM PHÍM 3 → ĐỔI SANG ĐẠN ACID
        if (Input.GetKeyDown(KeyCode.Alpha3) && canUseAcid)
        {
            TankShooting shooting = GetComponent<TankShooting>(); // Lấy script bắn
            if (shooting != null)
            {
                shooting.ChangeBullet(acidBulletPrefab, 5); // Đổi đạn + cấp 5 viên
                canUseAcid = false; // Dùng xong thì mất item
                GameEvents.OnAcidBulletCountChanged(false);
            }
        }

        // BẤM PHÍM 4 → ĐỔI SANG ĐẠN LỬA
        if (Input.GetKeyDown(KeyCode.Alpha4) && canUsefire)
        {
            TankShooting shooting = GetComponent<TankShooting>(); // Lấy script bắn
            if (shooting != null)
            {
                shooting.ChangeBullet(fireBulletPrefab, 5); // Đổi đạn + cấp 5 viên
                canUsefire = false; // Dùng xong thì mất item
                GameEvents.RaiseFireBulletStatusChanged(false);
            }
        }
    }

    // ===== CÁC HÀM ĐƯỢC GỌI KHI PLAYER NHẶT ITEM =====

    public void CollectShield()
    {
        canUseShield = true; // Cho phép dùng khiên
        Debug.Log("Đã nhặt Khiên Ô! Bấm 1 để kích hoạt.");
        GameEvents.RaiseShieldStatusChanged(true);
    }

    public void CollectHealth()
    {
        canUseHealth = true; // Cho phép dùng hồi máu
        GameEvents.RaiseHealthItemStatusChanged(true);
        Debug.Log("Đã nhặt heal");
    }

    public void CollectAxitBullet()
    {
        canUseAcid = true; // Cho phép dùng đạn acid
        GameEvents.OnAcidBulletCountChanged(true);
        Debug.Log("Đã nhặt đạn Acid! Bấm phím 3 để dùng.");
    }

    public void CollectFireBullet()
    {
        canUsefire = true; // Cho phép dùng đạn lửa
        GameEvents.RaiseFireBulletStatusChanged(true);
        Debug.Log("Đã nhặt fire, nhấn phím 4 để dùng");
    }

   
    private void UseUmbrella()
    {
        canUseShield = false; // Dùng xong thì mất item

        // Tạo khiên tại attachPoint
        GameObject shield = Instantiate(umbrellaShieldPrefab, attachPoint);
        GameEvents.RaiseShieldStatusChanged(false);
        // Căn vị trí khiên phía trước nòng súng
        shield.transform.localPosition = new Vector3(0f, 0f, 0.5f);
        shield.transform.localRotation = Quaternion.Euler(80f, 0f, 0f);

        // Tự hủy khiên sau thời gian định sẵn
        Destroy(shield, shieldDuration);
    } // ===== LOGIC DÙNG KHIÊN =====

    private void UseHealth()
    {
        canUseHealth = false; // Dùng xong thì mất item
        GameEvents.RaiseHealthItemStatusChanged(false);
        // Lấy script PlayerHealth trên Player
        PlayerHealth playerHealth = GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            // Bắt đầu hồi máu theo thời gian
            StartCoroutine(HealOverTimeRoutine(playerHealth));

            // Nếu có hiệu ứng hồi máu
            if (healthEff != null)
            {
                GameObject healEffect = Instantiate(
                    healthEff,
                    transform.position,
                    Quaternion.identity,
                    transform
                );

                // Hủy hiệu ứng sau khi hồi xong
                Destroy(healEffect, duration);
            }
        }
        else
        {
            Debug.LogError("Không tìm thấy script PlayerHealth trên Player!");
        }
    }// ===== LOGIC DÙNG HỒI MÁU =====

    
    private System.Collections.IEnumerator HealOverTimeRoutine(PlayerHealth playerHealth)// ===== COROUTINE HỒI MÁU THEO THỜI GIAN =====
    {
        float healedRemainder = 0f;                    // Phần lẻ chưa hồi
        float healRate = (float)totalHealAmount / duration; // Tốc độ hồi máu
        float timer = 0f;                              // Bộ đếm thời gian

        while (timer < duration)
        {
            // Nếu player chết trong lúc hồi → dừng ngay
            if (playerHealth == null || playerHealth.isDead)
                yield break;

            // Tính lượng hồi trong frame này
            float healThisFrame = healRate * Time.deltaTime;
            healedRemainder += healThisFrame;

            // Chỉ hồi khi đủ 1 đơn vị máu
            int healInt = Mathf.FloorToInt(healedRemainder);
            if (healInt > 0)
            {
                playerHealth.Heal(healInt); // Gọi hàm Heal
                healedRemainder -= healInt;
            }

            timer += Time.deltaTime; // Tăng thời gian
            yield return null;       // Chờ frame tiếp theo
        }

        // Hồi nốt phần dư nếu còn
        if (playerHealth != null && !playerHealth.isDead && healedRemainder >= 0.5f)
        {
            playerHealth.Heal(1);
        }
    }
}
