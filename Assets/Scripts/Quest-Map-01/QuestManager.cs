using UnityEngine;
using System.Collections.Generic;


public class QuestManager : MonoBehaviour
{
    [Header("Mission Info")]
    [TextArea(3, 5)]
    [SerializeField] private string missionDescription = "Nhiệm vụ: Vượt qua các checkpoint và tiêu diệt Boss!";
    private bool isBattleStarted = false;

    [Header("Quest Objects (Local References)")]
    public List<Transform> players;
    public Transform pointA, pointB, pointC;
    public GameObject bossPrefab;
    public Transform bossSpawnPoint;
    public GameObject checkpointEffect;

    [Header("UI References (In-Scene)")]
    public QuestTimerUI questTimerUI;
    public MissionUI missionUI;
    public QuestTextUI questTextUI;

    private List<Quest_Base> quests = new List<Quest_Base>();
    private int currentIndex = 0;
    private float totalLimit = 600f;
    private float totalPassed = 0f;
    private bool isFailed = false;
    private bool allCompleted = false;

    private QuestSpawnBoss spawnBossQuest;

    void Start()
    {
        // 1. Tự tìm Player nếu chưa có
        if (players == null || players.Count == 0)
        {
            players = new List<Transform>();
            foreach (var go in GameObject.FindGameObjectsWithTag("Player"))
                players.Add(go.transform);
        }

        // 2. Khởi tạo danh sách Quest địa phương
        SetupQuestChain();

        if (UIManager.Instance != null)
        {
            // TRUYỀN ĐÚNG: string (nhiệm vụ), sau đó là Action (hàm OnCountdownFinished)
            UIManager.Instance.ShowCountdown(missionDescription, OnCountdownFinished);
        }
        else
        {
            OnCountdownFinished();
        }
    }

    private void SetupQuestChain()
    {
        quests.Add(new QuestReachPoint("Đến điểm A", players, pointA, checkpointEffect));
        quests.Add(new QuestReachPoint("Đến điểm B", players, pointB, checkpointEffect));

        spawnBossQuest = new QuestSpawnBoss("Spawn Boss", bossPrefab, bossSpawnPoint);
        quests.Add(spawnBossQuest);

        quests.Add(new QuestReachPoint("Đến điểm C", players, pointC, checkpointEffect));

        foreach (var q in quests)
        {
            q.SetUI(missionUI);
            q.SetQuestTextUI(questTextUI);
        }
    }

    private void OnCountdownFinished()
    {
        isBattleStarted = true;
        if (quests.Count > 0) quests[currentIndex].StartQuest();
        Debug.Log("Trận đấu chính thức bắt đầu!");
    }

    void Update()
    {
        if (!isBattleStarted || isFailed || allCompleted) return;

        totalPassed += Time.deltaTime;

        if (totalPassed >= totalLimit)
        {
            isFailed = true;
            missionUI.ShowFailed();
            return;
        }

        if (currentIndex < quests.Count)
        {
            var current = quests[currentIndex];
            current.UpdateQuest();

            if (current.isCompleted) GoNextQuest();
        }

        float timeLeft = totalLimit - totalPassed;
        if (questTimerUI) questTimerUI.UpdateTimer(timeLeft, totalLimit);
    }

    void GoNextQuest()
    {
        if (quests[currentIndex] == spawnBossQuest)
        {
            var boss = spawnBossQuest.GetBoss();
            var killBossQuest = new QuestKillBoss("Tiêu diệt Boss", boss);
            killBossQuest.SetUI(missionUI);
            killBossQuest.SetQuestTextUI(questTextUI);
            quests.Insert(currentIndex + 1, killBossQuest);
        }

        currentIndex++;

        if (currentIndex >= quests.Count)
        {
            allCompleted = true;
            missionUI.ShowSuccess();
            return;
        }

        quests[currentIndex].StartQuest();
    }
}