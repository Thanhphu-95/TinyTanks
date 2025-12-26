using UnityEngine;

public class QuestKillBoss : Quest_Base
{
    private GameObject boss;

    public QuestKillBoss(string name, GameObject bossRef) : base(name)
    {
        this.questName = name;
        this.questText = "Nhiệm vụ: " + name;
        this.boss = bossRef;
    }

    public override void StartQuest()
    {
        base.StartQuest();
        //var bossScript = boss.GetComponent<BossController>();
        //if (bossScript != null)
        //{
        //    var ui = Object.FindObjectOfType<BossHealthUI>(true);
        //    if (ui != null) ui.ShowBossBar("TRÙM CUỐI", bossScript.maxHealth);
        //}

    }
    

    public override void UpdateQuest()
    {
        if (!isCompleted && boss == null) CompleteQuest();
    }

    public override void CompleteQuest() => isCompleted = true;
}