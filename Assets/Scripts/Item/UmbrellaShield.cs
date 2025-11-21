using UnityEngine;

public class UmbrellaShieldItem : SupportItem
{
    public GameObject shieldPrefab;
    private GameObject shieldInstance;
    public Transform shieldPoint;

    public override void Activate(GameObject target)
    {
        Transform turret = target.transform.Find("Turret");

        shieldInstance = Instantiate(shieldPrefab, shieldPoint);

        shieldInstance.transform.localPosition = new Vector3(0f, 0f, -0.5f);

        Quaternion customRotation = Quaternion.Euler(90f, 0f, 0f);
        shieldInstance.transform.localRotation = customRotation;

        //Destroy(shieldInstance, duration);
        //Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Kiểm tra đối tượng va chạm là viên đạn
        if (collision.gameObject.CompareTag("Bullet"))
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
