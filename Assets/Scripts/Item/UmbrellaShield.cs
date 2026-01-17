using UnityEngine;

public class UmbrellaShieldItem : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Lấy Rigidbody của đối tượng va chạm
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

        // Nếu đối tượng có Rigidbody thì phản xạ
        if (rb != null)
        {
            Vector3 incomingVelocity = rb.linearVelocity;                  // Vận tốc hiện tại
            Vector3 normal = collision.contacts[0].normal;           // Pháp tuyến va chạm
            Vector3 reflectVelocity = Vector3.Reflect(incomingVelocity, normal);

            rb.linearVelocity = reflectVelocity;                            // Gán vận tốc phản xạ
        }
    }
}
