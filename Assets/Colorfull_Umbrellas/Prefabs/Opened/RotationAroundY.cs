using UnityEngine;

public class RotateAroundY : MonoBehaviour
{
    // Tốc độ xoay, đơn vị: độ/giây
    public float rotationYSpeed = 0f;
    public float rotationXSpeed = 0f;

    void Update()
    {
        // Xoay quanh trục Y theo chiều lên (Vector3.up)
        transform.Rotate(0f, rotationYSpeed * Time.deltaTime, 0f);
        transform.Rotate(rotationXSpeed * Time.deltaTime, 0f, 0f);
    }
}
