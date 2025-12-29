using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    private BossHealth health;
    private BossMove bossMove;
    private BossAttack attack;

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

    }

    // Update is called once per frame
    void Update()
    {
        int curentHeal = health.currentHealth;
        if (health.isDead || player == null) return;
        
        if (curentHeal >= health.maxHealth * 0.9)
        {
            bossMove.TurretLookAt();
            Phase01();
        }
        else if (curentHeal > health.maxHealth *0.5)
        {
            Phase02();
        }
        else
        { 
            Phase03();
        }

        if (curentHeal < health.maxHealth * 0.3)
        {
            Destroy(sheild);
        }

    }



    private void Phase01()
    {
        
        attack.LaunchArcShot(player);
    }


    private void Phase02()
    {

    }

    private void Phase03()
    {

    }
}
