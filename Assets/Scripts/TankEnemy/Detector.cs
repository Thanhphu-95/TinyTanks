using UnityEngine;

public class Detector : MonoBehaviour
{
    public float detectionRange = 15f;
    public float detectionAngle = 45f;
    public LayerMask playerLayer;
    public Transform turret;

    // Phát hiện player, trả về true + player nếu có
    public bool DetectPlayer(out Transform player)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRange, playerLayer);
        foreach (var hit in hits)
        {
            Vector3 dirToPlayer = (hit.transform.position - turret.position).normalized;
            float angle = Vector3.Angle(turret.forward, dirToPlayer);

            if (angle <= detectionAngle)
            {
                player = hit.transform;
                return true;
            }
        }
        player = null;
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        if (turret == null)
        {
            turret = transform; // fallback nếu chưa gán
        }
        // Vẽ bán kính phát hiện
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Vẽ góc quét
        Vector3 leftLimit = Quaternion.Euler(0, -detectionAngle, 0) * turret.forward;
        Vector3 rightLimit = Quaternion.Euler(0, detectionAngle, 0) * turret.forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(turret.position, turret.position + leftLimit * detectionRange);
        Gizmos.DrawLine(turret.position, turret.position + rightLimit * detectionRange);
    }
}
