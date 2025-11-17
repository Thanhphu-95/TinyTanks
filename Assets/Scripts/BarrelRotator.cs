using UnityEngine;

public class BarrelRotator : MonoBehaviour
{
    public Transform barrel;    // Kéo Barrel vào đây
    public float rotateSpeed = 20f;
    public float minAngle;   // hạ nòng tối đa
    public float maxAngle = 30f;   // nâng nòng tối đa

    public LineRenderer line;
    public float laserLength = 20f;

    float currentAngle = 0f;

    void Update()
    {
        // Scroll mouse (giá trị thường là -1, 0, 1)
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0f)
        {
            // tăng/giảm góc theo chiều scroll
            currentAngle -= scroll * rotateSpeed;

            // giới hạn
            currentAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);

            // áp dụng
            barrel.localRotation = Quaternion.Euler(currentAngle, 0, 0);
        }

        Line();
    }


    public void Line()
    {
        // Điểm đầu nên lấy từ đầu nòng súng (firePoint nếu có)
        Vector3 start = barrel.position;

        // HƯỚNG LASER phải lấy từ barrel!
        Vector3 direction = barrel.forward;
        // hoặc barrel.right nếu mô hình của bạn bắn theo trục X

        Vector3 end = start + direction * laserLength;

        line.SetPosition(0, start);
        line.SetPosition(1, end);
    }
}
