using UnityEngine;
using System.Collections.Generic;

public class GatlingShooting : MonoBehaviour
{
    public Transform target; //tham chiếu đến Transform của Player
    public GameObject bulletPrefab;
    public List<Transform> firePoints;
    public float fireRate = 0.5f;
    public float speed = 5f;            // Tốc độ này sẽ là tốc độ bay của đạn
    private float nextFire = 0f;
    private int currentFireIndex = 0;

    private void Update()
    {
    }

    public void LogicShoot()
    {
        if (Time.time < nextFire) return;
        if (firePoints.Count > 0 && target != null) // Thêm kiểm tra target
        {
            Shoot();
            nextFire = Time.time + fireRate;
        }
    }

    public void Shoot()
    {
        Debug.Log("bắn đạn");

        if (target == null)
        {
            Debug.Log("Không tìm thấy mục tiêu (target) để bắn!");
            return;
        }

        Transform firePoint = firePoints[currentFireIndex];
        if (firePoint != null && bulletPrefab != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb == null)
            {
                Destroy(bullet);
                return;
            }

            
            Vector3 directionToTarget = (target.position - firePoint.position).normalized;// Vector hướng từ điểm bắn đến vị trí Player
            rb.AddForce(directionToTarget * speed, ForceMode.Impulse); // thêm lực bắn

            bullet.transform.rotation = Quaternion.LookRotation(directionToTarget); // xoay hướng đạn
        }

        currentFireIndex = (currentFireIndex + 1) % firePoints.Count;
    }
}