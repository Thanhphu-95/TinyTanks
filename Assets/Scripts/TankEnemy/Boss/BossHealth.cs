using System;
using System.Threading;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    [Header("Chỉ số máu")]
    public int maxHealth = 100;
    public int currentHealth;

    public bool isDead = false;

    // Sự kiện báo hiệu khi máu thay đổi (truyền % máu cho UI) hoặc khi chết
    public event Action<float> OnHealthPercentChanged;
    public event Action OnDeath;

    [Header("Phase destroy")]
    public Transform arm;                 // Cánh tay đang gắn trên boss
    public GameObject armBrokenPrefab;    // Prefab cánh tay rơi
    private bool armBroken = false;

    public Transform arm2;
    public GameObject arm2BrokenPrefab;
    private bool arm2Broken = false;

    public Transform shield;
    public GameObject shieldBrokenPrefab;
    private bool shieldBroken = false;


    public GameObject explosionPrefab;


    void Start()
    {
        currentHealth = maxHealth;

        // Đăng ký sự kiện
        OnHealthPercentChanged += HandleHealthUI;
        OnDeath += HandleBossDeath;

        // Khởi tạo UI trên màn hình (Ví dụ tên Boss là "UFO MOTHER SHIP")
        if (InGameUIManager.Instance != null)
        {
            InGameUIManager.Instance.InitBossHealthBar("UFO MOTHER SHIP", 1f);
        }
    }
    private void Update()
    {
        if(isDead) return;
        float percent= (float)currentHealth/maxHealth;

        if (!armBroken && percent <= 0.8f)
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.library.Explode);
            }
            BreakArm();
            armBroken = true;
        }
        if (!arm2Broken && percent <= 0.4f)
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.library.Explode);
            }
            BreakArm2();
            arm2Broken = true;
        }
        if (!shieldBroken && percent <= 0.2f)
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.library.Explode);
            }
            BreakShield();
            shieldBroken = true;
        }
    }
    private void HandleHealthUI(float percent)
    {
        if (InGameUIManager.Instance != null)
        {
            InGameUIManager.Instance.UpdateBossHealth(percent);
        }
    }

    private void HandleBossDeath()
    {
        if (InGameUIManager.Instance != null)
        {
            InGameUIManager.Instance.HideBossHealthBar();
        }
        // Các logic nổ tung, rơi quà...
    }

    private void OnDestroy()
    {
        // Hủy đăng ký để tránh lỗi bộ nhớ
        OnHealthPercentChanged -= HandleHealthUI;
        OnDeath -= HandleBossDeath;
    }

    // Hàm nhận sát thương
    public void TakeDamage(int damage)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.library.metaHit);
        }
        if (isDead) return;

        currentHealth -= damage;

        // Đảm bảo máu không âm
        if (currentHealth < 0) currentHealth = 0;

        // Tính toán tỷ lệ % để gửi cho thanh máu UI (0.0f đến 1.0f)
        float healthPercent = (float)currentHealth / maxHealth;
        OnHealthPercentChanged?.Invoke(healthPercent);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.library.Explode);
        }
        isDead = true;
        OnDeath?.Invoke();
        Debug.Log("Boss Die!");
        Destroy(gameObject);
    }
    void BreakArm()
    {
        if (arm == null) return;

        Vector3 pos = arm.position;
        Quaternion rot = arm.rotation;

        Destroy(arm.gameObject);

        if (explosionPrefab != null)
            Instantiate(explosionPrefab, pos, rot);

        if (armBrokenPrefab != null)
            Instantiate(armBrokenPrefab, pos, rot);
    }
    void BreakArm2()
    {
        Vector3 pos = arm2.position;
        Quaternion rot = arm2.rotation;
        Destroy(arm2.gameObject);
        if (explosionPrefab != null)
            Instantiate(explosionPrefab, pos, rot);

        if (armBrokenPrefab != null)
            Instantiate(arm2BrokenPrefab, pos, rot);

    }

    void BreakShield()
    {
        Vector3 pos = shield.position;
        Quaternion rot = shield.rotation;
        Destroy(shield.gameObject);
        if (explosionPrefab != null)
            Instantiate(explosionPrefab, pos, rot);

        if (armBrokenPrefab != null)
            Instantiate(shieldBrokenPrefab, pos, rot);
    }
}