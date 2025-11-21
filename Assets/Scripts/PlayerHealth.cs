using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Die")]
    public Transform barrel;
    public Transform turret;
    public GameObject barrelPrefab;
    public GameObject turretPrefab;


    void Start()
    {
        currentHealth = maxHealth;
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log("Player Healed: +" + amount + " | Current HP: " + currentHealth);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Player Damaged: -" + amount + " | Current HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(barrel.gameObject);
        Destroy(turret.gameObject);

        Instantiate(barrelPrefab, barrel.position, barrel.rotation);
        Instantiate(turretPrefab, turret.position, turret.rotation);


    }

    

}
