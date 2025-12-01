using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float liftTime = 5f;
    public int damage = 20;

    private Rigidbody rb;
    public LayerMask collisionLayers;
    public GameObject explosionPrefab;
    public GameObject TankHitPrefab;

    [Header("Explosion")]
    public float explosionRadius = 6f;
    public float explosionForce = 1500f;
    public float upwardModifier = 0.3f;  // lực hất lên
    public LayerMask explosionAffectLayers;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, liftTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlayerShield"))
        {
            return;
        }
        if (((1 << collision.gameObject.layer) & collisionLayers) != 0)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))
        {
            Instantiate(TankHitPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);

            PlayerHealth playerHP = collision.gameObject.GetComponent<PlayerHealth>();
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
            if (playerHP != null)
            {
                playerHP.TakeDamage(damage);
            }
            Explode();
        }
        
    }


    private void Explode()
    {
        

        // Lấy tất cả vật trong vùng nổ
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius, explosionAffectLayers);

        foreach (var hit in hits)
        {
            Rigidbody hitRb = hit.attachedRigidbody;
            if (hitRb != null)
            {
                hitRb.AddExplosionForce(
                    explosionForce,
                    transform.position,
                    explosionRadius,
                    upwardModifier,
                    ForceMode.Impulse
                );
            }

            // Nếu object có máu
            PlayerHealth hp = hit.GetComponent<PlayerHealth>();
            if (hp != null)
            {
                hp.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }
}
