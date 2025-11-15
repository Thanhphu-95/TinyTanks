using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 200f;

    [Header("Turret Settings")]
    public Transform turret;        // gắn phần Turret của tank
    public LayerMask groundMask;    // layer mặt đất (ví dụ "Ground")
    public float turretRotateSpeed = 10f;

    private Rigidbody rb;
    private Vector3 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = false; // cho phép xe nghiêng khi va chạm
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
    }

    void Update()
    {
        // --- Điều khiển di chuyển ---
        float moveInput = Input.GetAxis("Vertical");   // W / S
        float rotateInput = Input.GetAxis("Horizontal"); // A / D

        moveDirection = transform.forward * moveInput * moveSpeed;
        transform.Rotate(Vector3.up * rotateInput * rotationSpeed * Time.deltaTime);

        // --- Lật lại xe ---
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetRotation();
        }

        // --- Xoay turret theo hướng chuột ---
        RotateTurretToMouse();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveDirection * Time.fixedDeltaTime);
    }

    void ResetRotation()
    {
        Quaternion upright = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        rb.MoveRotation(upright);
        rb.angularVelocity = Vector3.zero;
    }

    void RotateTurretToMouse()
    {
        if (turret == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f, groundMask))
        {
            Vector3 lookPoint = hit.point;

            // Vector hướng từ turret tới điểm chuột
            Vector3 dir = lookPoint - turret.position;
            dir.y = 0; // Xoay ngang theo world

            if (dir.sqrMagnitude > 0.1f)
            {
                // Tính rotation theo world up (giữ turret không nghiêng theo thân xe)
                Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);

                // Xoay bằng localRotation để giữ đúng vị trí trên thân xe
                turret.localRotation = Quaternion.Lerp(
                    turret.localRotation,
                    targetRot,
                    Time.deltaTime * turretRotateSpeed
                );
            }
        }
    }

}
