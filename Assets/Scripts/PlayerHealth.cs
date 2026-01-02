using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Die Visuals")]
    public Transform barrel; public Transform turret; public Transform explode;
    public GameObject barrelPrefab; public GameObject turretPrefab; public GameObject explodePrefab;

    private Rigidbody rb;
    public bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
        GameEvents.OnPlayerHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        GameEvents.OnPlayerHealthChanged?.Invoke(currentHealth, maxHealth);
        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.library.Explode);
        }
        if (isDead) return;
        isDead = true;
        GetComponent<PlayerMovement>().enabled = false;
        if (rb) rb.isKinematic = true;
        Destroy(barrel.gameObject); Destroy(turret.gameObject);
        Instantiate(explodePrefab, explode.position, explode.rotation);
        Instantiate(barrelPrefab, barrel.position, barrel.rotation);
        Instantiate(turretPrefab, turret.position, turret.rotation);
        StartCoroutine(ShowFailedAfterDelay());
    }

    IEnumerator ShowFailedAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        if (InGameUIManager.Instance != null) InGameUIManager.Instance.ShowEndGame(false);
    }
    public void Heal(int amount)
    {
        if (isDead) return;

        // Cộng máu và đảm bảo không vượt quá maxHealth
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        // PHÁT SỰ KIỆN: Để InGameUIManager (trong Prefab) tự động cập nhật thanh máu
        GameEvents.OnPlayerHealthChanged?.Invoke(currentHealth, maxHealth);

        //Debug.Log("Đã hồi: " + amount + " HP. Máu hiện tại: " + currentHealth);
    }
}