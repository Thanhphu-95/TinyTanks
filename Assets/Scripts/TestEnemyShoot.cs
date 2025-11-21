using UnityEngine;

public class TestEnemyShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;

    public float shotForce = 800f;
    public float fireRate = 0.4f;

    private float nextFire = 0f;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
            Shoot();
    }

    void Shoot()
    {
        if (Time.time < nextFire) return;
        nextFire = Time.time + fireRate;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();


        // THÊM LỰC BẮN VẬT LÝ
        rb.AddForce(firePoint.forward * shotForce, ForceMode.Impulse);
    }
}
