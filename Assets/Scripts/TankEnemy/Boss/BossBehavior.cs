using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    private BossHealth health;
    private BossAttack attack;
    private BossMove bossMove;
    public Transform player;
    public GameObject sheild;

    private void Awake()
    {
        health = GetComponent<BossHealth>();
        bossMove = GetComponent<BossMove>();
        attack = GetComponent<BossAttack>();
    }
    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        int curentHeal = health.currentHealth;
        if (health.isDead || player == null) return;
        bossMove.HandleTurret();
        if (curentHeal >= health.maxHealth * 0.9)
        {
            
            Phase01();
        }
        else if (curentHeal > health.maxHealth *0.5)
        {
            bossMove.HandleMovement();
            Phase02();
        }
        else
        { 
            bossMove.HandleMovement();
            Phase03();
        }

        if (curentHeal < health.maxHealth * 0.3)
        {
            Destroy(sheild);
        }

    }

    private void Phase01()
    {
        attack.SingleShot();
    }

    private void Phase02()
    {
        
        attack.SmokePoof();
        attack.ArcBullet(player);
    }

    private void Phase03()
    {
            attack.SuiscideDrones(player);
        attack.SmokePoof();
        attack.ArcBullet(player);
    }
}
