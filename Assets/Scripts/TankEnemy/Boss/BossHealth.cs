using UnityEngine;
using System;

public class BossHealth : MonoBehaviour
{
    [Header("Chỉ số máu")]
    public int maxHealth = 100;
    public int currentHealth;

    public bool isDead = false;

    // Sự kiện báo hiệu khi máu thay đổi (truyền % máu cho UI) hoặc khi chết
    public event Action<float> OnHealthPercentChanged;
    public event Action OnDeath;

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
    }
}