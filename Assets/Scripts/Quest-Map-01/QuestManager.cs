using UnityEngine;                               // Dùng API của Unity
using System.Collections.Generic;                // Dùng List<T>

public class QuestManager : MonoBehaviour        // Script quản lý toàn bộ hệ thống nhiệm vụ
{
    public List<Transform> players;              // Danh sách player trong game (1 hoặc nhiều người)
    public Transform pointA, pointB, pointC;     // Ba checkpoint A B C
    public GameObject bossPrefab;                // Prefab của Boss để spawn
    public Transform bossSpawnPoint;             // Vị trí sẽ spawn boss

    private List<Quest_Base> quests =            // Danh sách tất cả quest theo thứ tự
        new List<Quest_Base>();
    private int currentIndex = 0;                // Quest đang chạy hiện tại (index trong list)

    private float totalLimit = 600f;             // Giới hạn thời gian 10 phút
    private float totalPassed = 0f;              // Thời gian đã trôi qua
    private bool isFailed = false;               // Flag: nhiệm vụ bị thất bại
    private bool allCompleted = false;           // Flag: tất cả quest đã hoàn thành

    private QuestSpawnBoss spawnBossQuest;       // Lưu quest spawn boss để tạo quest kill boss
    private QuestKillBoss killBossQuest;         // Lưu quest giết boss

    public GameObject checkpointEffect;          // Effect khi người chơi đến checkpoint
    [Header("UI")]
    public QuestTimerUI questTimerUI;
    public MissionUI missionUI;
    public QuestTextUI questTextUI;


    void Start()
    {
       
        if (players == null || players.Count == 0) // Nếu chưa kéo Player vào Inspector
        {
            players = new List<Transform>();        // Tạo list rỗng
            GameObject[] found =                    // Tìm tất cả object tag Player
                GameObject.FindGameObjectsWithTag("Player");
            foreach (var go in found)               // Thêm vào list
                players.Add(go.transform);
        }

        var q1 = new QuestReachPoint(               // Quest 1: đi đến điểm A
            "Đến điểm A", players, pointA, checkpointEffect);
         
        var q2 = new QuestReachPoint(               // Quest 2: đi đến điểm B
            "Đến điểm B", players, pointB, checkpointEffect);

        spawnBossQuest = new QuestSpawnBoss(        // Quest 3: spawn boss
            "Spawn Boss", bossPrefab, bossSpawnPoint);

        var q4 = new QuestReachPoint(               // Quest 4: đi đến điểm C
            "Đến điểm C", players, pointC, checkpointEffect);

        quests.Add(q1);                             // Thêm vào danh sách theo thứ tự 
        quests.Add(q2);
        quests.Add(spawnBossQuest);
        quests.Add(q4);
        foreach (var q in quests)
        {
            q.SetUI(missionUI);           // Gán MissionUI vào từng quest
            q.SetQuestTextUI(questTextUI);
        }
        quests[currentIndex].StartQuest();          // Bắt đầu quest đầu tiên
    }

    void Update()
    {
        if (isFailed || allCompleted) return;       // Nếu fail hoặc xong hết → không chạy nữa

        totalPassed += Time.deltaTime;              // Cộng thời gian mỗi frame

        if (totalPassed >= totalLimit)              // Nếu hết 10 phút
        {
            isFailed = true;                        // Đánh dấu thất bại
            Debug.Log("Nhiệm vụ thất bại: Hết 10 phút!");
            missionUI.ShowFailed(); 
            return;
        }

        if (currentIndex < 0 ||                    // Nếu index bị sai
            currentIndex >= quests.Count)          // Hoặc vượt khỏi list
            return;                                // → Dừng để không lỗi OutOfRange

        var current = quests[currentIndex];         // Lấy quest hiện tại
        current.UpdateQuest();                      // Gọi Update() của quest đó

        if (current.isCompleted)                    // Nếu quest đã hoàn thành
        {
            GoNextQuest();                          // Chuyển qua quest tiếp theo
        }

        float timeLeft = totalLimit - totalPassed;

        questTimerUI.UpdateTimer(timeLeft, totalLimit);

    }

    void GoNextQuest()
    {
        if (quests[currentIndex] == spawnBossQuest) // Nếu quest vừa làm là SpawnBoss
        {
            var boss = spawnBossQuest.GetBoss();    // Lấy boss vừa spawn
            killBossQuest = new QuestKillBoss(      // Tạo quest kill boss
                "Tiêu diệt Boss", boss);
            quests.Insert(currentIndex + 1,         // Chèn ngay sau quest spawn
                killBossQuest);
        }

        currentIndex++;                             // Chuyển sang quest kế tiếp

        if (currentIndex >= quests.Count)           // Nếu vượt danh sách quest
        {
            allCompleted = true;                    // Đánh dấu hoàn tất toàn bộ nhiệm vụ
            Debug.Log("🎉 Hoàn thành toàn bộ nhiệm vụ!");
            missionUI.ShowSuccess(); 
            return;                                 // Dừng luôn không chạy StartQuest()
        }

        quests[currentIndex].StartQuest();          // Bắt đầu quest kế tiếp
    }
}
