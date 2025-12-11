using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public PlayerUI playerUI;

    [Header("Die")]
    public Transform barrel;
    public Transform turret;
    public Transform explode;

    public GameObject barrelPrefab;
    public GameObject turretPrefab;
    public GameObject explodePrefab;
    private Rigidbody rb;

    public MissionUI missionUI;


    public bool isDead = false;


    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
        if (playerUI != null)
            playerUI.UpdateHP(currentHealth, maxHealth);
    }

    public void Heal(int amount)
    {
        if (isDead) return;
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        if (playerUI != null)
            playerUI.UpdateHP(currentHealth, maxHealth);

        Debug.Log("Player Healed: +" + amount + " | Current HP: " + currentHealth);
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (playerUI != null)
            playerUI.UpdateHP(currentHealth, maxHealth);

        Debug.Log("Player Damaged: -" + amount + " | Current HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;
        GetComponent<PlayerMovement>().enabled = false;
        rb.isKinematic = true;
        Destroy(barrel.gameObject);
        Destroy(turret.gameObject);
        Instantiate(explodePrefab, explode.position, explode.rotation);
        Instantiate(barrelPrefab, barrel.position, barrel.rotation);
        Instantiate(turretPrefab, turret.position, turret.rotation);
        StartCoroutine(ShowFailedAfterDelay());


    }
    private System.Collections.IEnumerator ShowFailedAfterDelay()
    {
        yield return new WaitForSeconds(3f);   // ⏳ Delay 3 giây

        if (missionUI != null)
            missionUI.ShowFailed();
    }


}
