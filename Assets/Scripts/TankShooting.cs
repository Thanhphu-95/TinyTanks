using UnityEngine;

public class TankShooting : MonoBehaviour
{

    public GameObject bulletPrefab;
    public Transform firePoint;

    public float shotForce = 800f;
    public float fireRate = 0.4f;

    private float nextFire = 0f;

    // Thêm các biến quản lý đạn đặc biệt
    private GameObject originalBulletPrefab;
    private int specialAmmoRemaining = 0;

    void Start()
    {
        originalBulletPrefab = bulletPrefab;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (Time.time < nextFire) return;
        nextFire = Time.time + fireRate;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * shotForce, ForceMode.Impulse);// THÊM LỰC BẮN VẬT LÝ
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.library.ShootBullet);
        }
        // Kiểm tra nếu đang dùng đạn đặc biệt thì trừ dần số lượng
        if (specialAmmoRemaining > 0)
        {
            specialAmmoRemaining--;

            // Nếu bắn hết 5 viên, quay về đạn cũ
            if (specialAmmoRemaining <= 0)
            {
                bulletPrefab = originalBulletPrefab;
                Debug.Log("Hết đạn Acid, quay lại đạn thường.");
            }
        }
    }
    // Hàm để ItemManager gọi khi bạn bấm phím 3
    public void ChangeBullet(GameObject newBullet, int ammoAmount)
    {
        bulletPrefab = newBullet;
        specialAmmoRemaining = ammoAmount;
        Debug.Log("Đã đổi sang đạn Acid! Số lượng: " + ammoAmount);
    }
}
