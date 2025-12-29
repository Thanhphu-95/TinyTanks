using UnityEngine;

public class UmbrellaShieldItem : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("BulletEnemy")) // Kiểm tra đối tượng va chạm là viên đạn
        {
            Rigidbody bulletRb = collision.gameObject.GetComponent<Rigidbody>();
            if (bulletRb != null)
            {
                // Lấy vector va chạm, phản xạ lại
                Vector3 incomingVelocity = bulletRb.linearVelocity;
                Vector3 normal = collision.contacts[0].normal;
                Vector3 reflectVelocity = Vector3.Reflect(incomingVelocity, normal);

                bulletRb.linearVelocity = reflectVelocity;

            }
        }
    }

}
