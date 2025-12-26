using UnityEngine;

public class QuestSpawnBoss : Quest_Base
{
    private GameObject bossPrefab;
    private Transform spawnPoint;
    private GameObject spawnedBoss;

    public GameObject GetBoss() => spawnedBoss;

    public QuestSpawnBoss(string name, GameObject prefab, Transform point) : base(name)
    {
        this.questName = name;
        this.questText = name; // Ví dụ: "Cảnh báo: Boss xuất hiện!"
        this.bossPrefab = prefab;
        this.spawnPoint = point;
    }

    public override void StartQuest()
    {
        base.StartQuest();
        if (bossPrefab != null && spawnPoint != null)
        {
            spawnedBoss = Object.Instantiate(bossPrefab, spawnPoint.position, spawnPoint.rotation);
        }
        CompleteQuest();
    }

    public override void UpdateQuest() { }

    public override void CompleteQuest() => isCompleted = true;
}