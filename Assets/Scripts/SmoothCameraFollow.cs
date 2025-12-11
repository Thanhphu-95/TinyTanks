using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    public Transform target;        // mục tiêu camera theo dõi
    public Vector3 offset;          // khoảng cách cố định từ target
    [Range(0f, 1f)]
    public float positionSmooth = 0.1f;   // tốc độ nội suy vị trí (càng nhỏ càng mượt)
    [Range(0f, 1f)]
    public float rotationSmooth = 0.1f;   // tốc độ nội suy góc quay

    void LateUpdate()
    {
        if (target == null)
            return;
       
        // Vị trí mong muốn
        Vector3 desiredPosition = target.position + offset;
        // Nội suy vị trí camera từ vị trí hiện tại đến vị trí mong muốn
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, positionSmooth);
        transform.position = smoothedPosition;

        // Hướng camera về phía mục tiêu
        Quaternion desiredRotation = Quaternion.LookRotation(target.position - transform.position);
        // Nội suy góc quay camera
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSmooth);
    }
}
