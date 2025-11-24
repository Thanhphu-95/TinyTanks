using UnityEngine;
using System.Collections.Generic;

public class GatlingShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public List<Transform> firePoints;  // danh sách điểm bắn
    public float fireRate = 0.5f;       // giãn cách giữa các lần bắn (giây)
    public float speed = 5f;
    private float nextFire = 0f;
    private int currentFireIndex = 0;

    private void Update()
    {
    }

    public void LogicShoot()
    {

        if(Time.time < nextFire) return;
        if (firePoints.Count > 0)
        {
            Shoot();
            nextFire =  Time.time + fireRate;
        }
    }
    public void Shoot()
    {
        Transform firePoint = firePoints[currentFireIndex];
        if (firePoint != null && bulletPrefab != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();


            // THÊM LỰC BẮN VẬT LÝ
            rb.AddForce(firePoint.forward * speed, ForceMode.Impulse);
        }


        currentFireIndex = (currentFireIndex + 1) % firePoints.Count;
    }
}
