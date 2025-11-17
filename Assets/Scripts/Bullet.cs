using UnityEngine;

public class Bullet : MonoBehaviour
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

    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & collisionLayers) != 0)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);

        }

    }
}
