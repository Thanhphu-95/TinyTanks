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
        isDead = true;
        OnDeath?.Invoke();
        Debug.Log("Boss Die!");
    }
}