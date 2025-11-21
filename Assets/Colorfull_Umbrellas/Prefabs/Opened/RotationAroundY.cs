using UnityEngine;

public class RotateAroundY : MonoBehaviour
{
    // Tốc độ xoay, đơn vị: độ/giây
    public float rotationSpeed = 60f;

    void Update()
    {
        // Xoay quanh trục Y theo chiều lên (Vector3.up)
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}
