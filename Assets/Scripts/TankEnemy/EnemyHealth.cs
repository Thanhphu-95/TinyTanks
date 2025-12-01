using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health;
    public int currentHealth;
    public GameObject explode;


    [Header("Die")]
    private bool isDead;
    private Rigidbody rb;
    void Start()
    {
        currentHealth = health;
        rb = GetComponent<Rigidbody>();
    }
    public void TakeDamage(int amount)
    {
        if (isDead) return;
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, health);
        Debug.Log("Enemy Damaged: -" + amount + " | Enemy HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        if (isDead) return;
        isDead = true;
        rb.isKinematic = true;
        if (explode != null)
        {
            Instantiate(explode, transform.position, Quaternion.identity);
        }
        foreach (var renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = false;
        }
        Destroy(gameObject, 1f);

    }
}
