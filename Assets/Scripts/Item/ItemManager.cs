using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [Header("UmbrellaShield")]
    public GameObject umbrellaShieldPrefab; // Kéo Prefab có gắn UmbrellaShieldItem vào đây
    public Transform attachPoint;           // Thường là nòng súng hoặc Turret
    public float shieldDuration = 3f;       // Thời gian khiên tồn tại
    private bool canUseShield = false;      // Trạng thái đã nhặt được hay chưa

    [Header("UmbrellaShield")]
    public GameObject healthEff;
    public int totalHealAmount = 40;
    public float duration = 1f;

    private bool canUseHealth = false;


    [Header("AxitBullet")]
    public GameObject acidBulletPrefab;
    private bool canUseAcid = false;

    [Header("FireBullet")]
    public GameObject fireBulletPrefab;
    private bool canUsefire = false;


    public bool CanUseShield() => canUseShield;
    public bool CanUseHealth() => canUseHealth;
    public bool CanUseAcid() => canUseAcid;
    public bool CanUseFire() => canUsefire;
    void Update()
    {
        // 1. Kiểm tra bấm phím 1 và xem có khiên để dùng không
        if (Input.GetKeyDown(KeyCode.Alpha1) && canUseShield)
        {
            UseUmbrella();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && canUseHealth)
        {
            UseHealth();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && canUseAcid)
        {
            TankShooting shooting = GetComponent<TankShooting>();
            if (shooting != null)
            {
                shooting.ChangeBullet(acidBulletPrefab, 5); // Đổi đạn và cấp 5 viên
                canUseAcid = false; // Dùng xong thì mất trong kho
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && canUsefire)
        {
            TankShooting shooting = GetComponent<TankShooting>();
            if (shooting != null)
            {
                shooting.ChangeBullet(fireBulletPrefab, 5); // Đổi đạn và cấp 5 viên
                canUsefire = false; // Dùng xong thì mất trong kho
            }
        }
    }

    // 2. Hàm này để các vật phẩm trên đất gọi khi va chạm với Player
    public void CollectShield()
    {
        canUseShield = true;
        Debug.Log("Đã nhặt Khiên Ô! Bấm 1 để kích hoạt.");
    }

    public void CollectHealth()
    {
        Debug.Log("đã nhặt heal");
        canUseHealth = true;
    }

    public void CollectAxitBullet()
    {
        canUseAcid = true;
        Debug.Log("Đã nhặt đạn Acid! Bấm phím 3 để dùng.");
    }

    public void CollectFireBullet()
    {
        canUsefire = true;
        Debug.Log("đã nhặt fire, nhấn phím 4 để dùng");
    }


    private void UseUmbrella() //Logic kích hoạt khiên
    {
        canUseShield = false; // Dùng xong thì mất (không còn trong kho)

        // Sinh ra khiên tại vị trí attachPoint (nòng súng)
        GameObject shield = Instantiate(umbrellaShieldPrefab, attachPoint);

        // Chỉnh vị trí và góc xoay cho đúng hướng nòng súng
        shield.transform.localPosition = new Vector3(0f, 0f, 0.5f); // Đẩy ra trước nòng một chút
        shield.transform.localRotation = Quaternion.Euler(80f, 0f, 0f);

        // Tự động xóa khiên sau thời gian quy định
        Destroy(shield, shieldDuration);
    }

    private void UseHealth()
    {
        canUseHealth = false; // Sử dụng xong thì mất vật phẩm

        // Lấy script PlayerHealth trên chính đối tượng này
        PlayerHealth playerHealth = GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            // Bắt đầu quá trình hồi máu
            StartCoroutine(HealOverTimeRoutine(playerHealth));

            // Nếu bạn có hiệu ứng hình ảnh (ví dụ: vòng sáng xanh)
            if (healthEff != null)
            {
                // Sinh ra hiệu ứng tại vị trí Player
                GameObject healEffect = Instantiate(healthEff, transform.position, Quaternion.identity, transform);
                // Tự xóa hiệu ứng sau khi hết thời gian hồi máu
                Destroy(healEffect, duration);
            }
        }
        else
        {
            Debug.LogError("Không tìm thấy script PlayerHealth trên Player!");
        }
    }

    private System.Collections.IEnumerator HealOverTimeRoutine(PlayerHealth playerHealth)
    {
        float healedRemainder = 0f;
        float healRate = (float)totalHealAmount / duration;
        float timer = 0f;

        while (timer < duration)
        {
            // Nếu Player bỗng nhiên chết trong lúc đang hồi, dừng ngay lập tức
            if (playerHealth == null || playerHealth.isDead) yield break;

            float healThisFrame = healRate * Time.deltaTime;
            healedRemainder += healThisFrame;

            int healInt = Mathf.FloorToInt(healedRemainder);
            if (healInt > 0)
            {
                // Gọi hàm Heal đã viết trong PlayerHealth
                playerHealth.Heal(healInt);
                healedRemainder -= healInt;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        // Hồi nốt phần dư cuối cùng nếu còn sót lại > 0.5 đơn vị máu
        if (playerHealth != null && !playerHealth.isDead && healedRemainder >= 0.5f)
        {
            playerHealth.Heal(1);
        }
    }
}