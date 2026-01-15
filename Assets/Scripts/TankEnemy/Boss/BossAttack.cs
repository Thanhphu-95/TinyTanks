using UnityEngine;

public class BossAttack : MonoBehaviour
{
    [Header("Bắn thường (Phase 1)")]
    public GameObject bulletPrefab;
    public Transform firePointLeft;
    public Transform firePointRight;
    public float fireRate = 0.2f;
    private float shotTimer = 0f;
    private float nextFireTime = 0f;
    private bool isLeftTurn = true;

    [Header("Kỹ năng 2: Arc Bullet (Vòng cung)")]
    public GameObject arcBulletPrefab;
    public Transform firePointAttack2;
    public float arcCooldown = 6f;
    private float arcCooldownTimer = 0f;

    [Header("Ultimate: Suicide Drones (Drone tự sát)")]
    public GameObject dronePrefab;
    public Transform[] droneSpawnPoints;
    public float droneUltimateCooldown = 20f;
    private float droneTimer = 0f;

    [Header("Kỹ năng nhả khói (Smoke Poof)")]
    public GameObject smokePoofVFX;
    public Transform smokePoint;
    private float smokeTimer;
    private float currentRandomDelay;

    // --- CÁC HÀM TẤN CÔNG ---

    public void SingleShot()
    {
        //if (AudioManager.Instance != null)
        //{
        //    AudioManager.Instance.PlaySFX(AudioManager.Instance.library.bossSoot);
        //}
        shotTimer += Time.deltaTime;
        if (shotTimer >= 5f) shotTimer = 0f;

        if (shotTimer <= 3.5f && Time.time >= nextFireTime)
        {
            FireAlternating();
            nextFireTime = Time.time + fireRate;
        }
    }

    public void ArcBullet(Transform targetPlayer)
    {

        if (Time.time < arcCooldownTimer || targetPlayer == null) return;

        GameObject bulletObj = Instantiate(arcBulletPrefab, firePointAttack2.position, firePointAttack2.rotation);
        ArcBullet script = bulletObj.GetComponent<ArcBullet>();
        if (script != null)
        {
            script.Initialize(firePointAttack2.position, targetPlayer.position);
        }
        arcCooldownTimer = Time.time + arcCooldown;
    }

    public void SuiscideDrones(Transform targetPlayer)
    {
        if (Time.time < droneTimer || targetPlayer == null) return;

        for (int i = 0; i < droneSpawnPoints.Length; i++)
        {
            if (droneSpawnPoints[i] == null) continue;
            GameObject droneObj = Instantiate(dronePrefab, droneSpawnPoints[i].position, droneSpawnPoints[i].rotation);
            HomingMissile script = droneObj.GetComponent<HomingMissile>();
            if (script != null) script.Initialize(targetPlayer);
        }
        droneTimer = Time.time + droneUltimateCooldown;
    }

    public void SmokePoof()
    {
        if (currentRandomDelay == 0) currentRandomDelay = Random.Range(1f, 5f);
        smokeTimer += Time.deltaTime;

        if (smokeTimer >= currentRandomDelay)
        {
            if (smokePoofVFX != null && smokePoint != null)
            {
                GameObject smoke = Instantiate(smokePoofVFX, smokePoint.position, Quaternion.identity);
                //Destroy(smoke, 10f);
            }
            smokeTimer = 0f;
            currentRandomDelay = Random.Range(7f, 13f);
        }
    }

    // --- HÀM HỖ TRỢ ---

    private void FireAlternating()
    {
        Transform currentPoint = isLeftTurn ? firePointLeft : firePointRight;
        if (bulletPrefab != null && currentPoint != null)
        {
            Instantiate(bulletPrefab, currentPoint.position, currentPoint.rotation);
        }
        isLeftTurn = !isLeftTurn;
    }

    public void ResetAttackCycle()
    {
        shotTimer = 0f;
        nextFireTime = 0f;
    }
}