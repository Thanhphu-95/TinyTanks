using UnityEngine;                                         // Dùng API Unity

public class QuestSpawnBoss : Quest_Base                  // Quest để spawn boss
{
    private GameObject bossPrefab;                        // Prefab của boss
    private Transform spawnPoint;                         // Vị trí spawn boss
    private GameObject spawnedBoss;                       // Boss đã spawn ra

    public GameObject GetBoss() => spawnedBoss;           // Hàm trả boss vừa spawn cho quest sau

    public QuestSpawnBoss(string name, GameObject prefab, Transform point)
    {                                                     // Constructor khởi tạo quest
        this.questName = name;                            // Gán tên quest
        this.bossPrefab = prefab;                         // Gán prefab boss
        this.spawnPoint = point;                          // Gán điểm spawn
    }

    public override void StartQuest()                     // Gọi khi quest bắt đầu
    {
        Debug.Log("Bắt đầu: " + questName);               // Log bắt đầu quest
        spawnedBoss = GameObject.Instantiate(             // Tạo boss mới trong game
            bossPrefab,                                   // Prefab boss
            spawnPoint.position,                          // Vị trí spawn
            spawnPoint.rotation                           // Hướng spawn
        );
        CompleteQuest();                                  // Spawn xong → quest hoàn thành ngay
    }

    public override void UpdateQuest() { }                // Quest này không cần update mỗi frame

    public override void CompleteQuest()                  // Khi quest hoàn thành
    {
        isCompleted = true;                               // Đánh dấu hoàn thành
        Debug.Log("Boss đã xuất hiện!");                  // In ra thông báo
    }
}
