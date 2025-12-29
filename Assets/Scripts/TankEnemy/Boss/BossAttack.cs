using UnityEngine;

public class BossAttack : MonoBehaviour
{
    [Header("Settings")]
    public GameObject bulletPrefab;
    public Transform firePointLeft;
    public Transform firePointRight;
    public float fireRate = 0.2f; // Tốc độ bắn giữa mỗi viên
    public float bulletSpeed = 20f;

    [Header("Timer Logic")]
    private float shotTimer = 0f;      // Đếm thời gian trong chu kỳ
    private float nextFireTime = 0f;   // Nhịp bắn giữa 2 viên
    private bool isLeftTurn = true;    // Đổi nòng

    [Header("Kỹ năng 2: Arc Shot")]
    public GameObject arcBulletPrefab;
    public float arcCooldown = 3.0f; // Khoảng cách giữa các lần bắn (giây)
    private float arcCooldownTimer = 0f; // Bộ đếm thời gian nội bộ
    public Transform firePointAttack2;

    public void SingleShot()
    {
        // 1. Cập nhật bộ đếm thời gian chu kỳ (Tổng 5 giây: 3s bắn + 2s nghỉ)
        shotTimer += Time.deltaTime;

        // Nếu vượt quá 5s thì reset chu kỳ về 0
        if (shotTimer >= 5f)
        {
            shotTimer = 0f;
        }

        // 2. Kiểm tra nếu đang trong 3.5 giây đầu của chu kỳ thì mới bắn
        if (shotTimer <= 3.5f)
        {
            // Kiểm tra nhịp bắn (fireRate)
            if (Time.time >= nextFireTime)
            {
                FireAlternating();
                nextFireTime = Time.time + fireRate;
            }
        }
        // Sau 3s (tức là từ giây thứ 3.1 đến 5.0), hàm này sẽ không làm gì cả -> Tự động nghỉ 2s
    }
    public void LaunchArcShot(Transform targetPlayer)
    {
        // Kiểm tra nếu chưa đủ thời gian chờ thì không bắn
        if (Time.time < arcCooldownTimer || targetPlayer == null) return;

        // Sinh đạn
        GameObject bulletObj = Instantiate(arcBulletPrefab, firePointAttack2.position, Quaternion.identity);

        ArcBullet arcScript = bulletObj.GetComponent<ArcBullet>();
        if (arcScript != null)
        {
            arcScript.Initialize(firePointAttack2.position, targetPlayer.position);
        }

        // Cập nhật thời điểm được bắn lần tiếp theo
        arcCooldownTimer = Time.time + arcCooldown;     
    }


    private void FireAlternating()
    {
        // Chọn nòng súng
        Transform currentPoint = isLeftTurn ? firePointLeft : firePointRight;

        // Bắn đạn
        if (bulletPrefab != null && currentPoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, currentPoint.position, currentPoint.rotation);
        }

        // Đổi nòng cho lần gọi sau
        isLeftTurn = !isLeftTurn;
    }

    // Reset lại bộ đếm nếu cần (ví dụ khi chuyển Phase muốn Boss bắn ngay lập tức)
    public void ResetAttackCycle()
    {
        shotTimer = 0f;
        nextFireTime = 0f;
    }
}