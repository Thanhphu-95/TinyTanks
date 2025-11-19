using UnityEngine;

public class FireBullet : MonoBehaviour
{
    public float liftTime = 5f;
    public int damage = 20;

    private Rigidbody rb;
    public LayerMask collisionLayers;
    public GameObject explosionPrefab;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, liftTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & collisionLayers) != 0)
        {

            GameObject vfx = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(vfx, 5f);
            Destroy(gameObject);
        }

    }
}
