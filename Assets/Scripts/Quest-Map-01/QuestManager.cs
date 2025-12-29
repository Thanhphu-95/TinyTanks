using UnityEngine;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    [Header("Quest Objects")]
    public List<Transform> players;
    public Transform pointA, pointB, pointC;
    public GameObject bossPrefab, checkpointEffect;
    public Transform bossSpawnPoint;

    [Header("Time Limit Settings")]
    [SerializeField] private float totalLimit = 600f; // Giới hạn thời gian (giây)
    private float totalPassed = 0f;
    private bool isBattleStarted = false;
    private bool isFailed = false;

    private List<Quest_Base> quests = new List<Quest_Base>();
    private int currentIndex = 0;
    private QuestSpawnBoss spawnBossQuest;

    void Start()
    {
        // Tự động tìm Player nếu danh sách trống
        if (players == null || players.Count == 0)
        {
            players = new List<Transform>();
            foreach (var go in GameObject.FindGameObjectsWithTag("Player"))
                players.Add(go.transform);
        }

        SetupQuestChain();

        // Khởi động Countdown bắt đầu trận đấu
        if (UIManager.Instance != null)
            UIManager.Instance.ShowCountdown("Nhiệm vụ: chiếm đóng các vị trí và tiêu diệt kẻ thù", OnCountdownFinished);
        else
            OnCountdownFinished();
    }

    private void SetupQuestChain()
    {
        // Bước 1: Đến điểm A và giữ trong 5 giây
        quests.Add(new QuestReachPoint("Chiếm vị trí sân bay", players, pointA, checkpointEffect, 5f));

        // Bước 2: Đến điểm B và giữ trong 5 giây
        quests.Add(new QuestReachPoint("tấng công vị trí tiếp theo", players, pointB, checkpointEffect, 5f));

        // Bước 3: Spawn Boss
        spawnBossQuest = new QuestSpawnBoss("CẢNH BÁO: Viện binh của kẻ thù đã đến, hay tiêu diệt chúng", bossPrefab, bossSpawnPoint);
        quests.Add(spawnBossQuest);

        // Bước cuối: Rút lui về điểm C (giữ 1 giây)
        quests.Add(new QuestReachPoint("Điểm C (Rút lui)", players, pointC, checkpointEffect, 1f));
    }

    private void OnCountdownFinished()
    {
        isBattleStarted = true;
        if (InGameUIManager.Instance != null) InGameUIManager.Instance.ShowHUD(true);
        if (quests.Count > 0) quests[currentIndex].StartQuest();
    }

    void Update()
    {
        if (!isBattleStarted || isFailed || currentIndex >= quests.Count) return;

        // --- Xử lý giới hạn thời gian ---
        totalPassed += Time.deltaTime;
        float timeLeft = totalLimit - totalPassed;

        if (timeLeft >= 0)
        {
            GameEvents.OnQuestTimeUpdate?.Invoke(timeLeft);
        }
        else
        {
            isFailed = true;
            if (InGameUIManager.Instance != null) InGameUIManager.Instance.ShowEndGame(false);
            return;
        }

        // --- Cập nhật Quest hiện tại ---
        quests[currentIndex].UpdateQuest();

        if (quests[currentIndex].isCompleted)
        {
            GoNextQuest();
        }
    }

    void GoNextQuest()
    {
        // Nếu vừa xong Quest Spawn, chèn thêm Quest Kill Boss vào danh sách
        if (quests[currentIndex] == spawnBossQuest)
        {
            var killQuest = new QuestKillBoss("Tiêu diệt Boss", spawnBossQuest.GetBoss());
            quests.Insert(currentIndex + 1, killQuest);
        }

        currentIndex++;

        if (currentIndex >= quests.Count)
        {
            // Hoàn thành tất cả nhiệm vụ
            if (InGameUIManager.Instance != null) InGameUIManager.Instance.ShowEndGame(true);
            return;
        }

        // Bắt đầu Quest tiếp theo
        quests[currentIndex].StartQuest();
    }
}