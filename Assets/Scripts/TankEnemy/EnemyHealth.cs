using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health;
    public int currentHealth;
    public GameObject explode;
    public GameObject fire;
    public Transform pointFire;


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
        if (currentHealth <= health/2)
        {
          
            if (pointFire.childCount == 0)
            {
                GameObject flame = Instantiate(fire, pointFire.position, pointFire.rotation);
                flame.transform.SetParent(pointFire);
            }

        }
        
    }
    private void Die()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.library.Explode);
        }
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
