using UnityEngine;
using System.Collections.Generic;

public class QuestReachPoint : Quest_Base
{
    private List<Transform> players;
    private Transform target;
    private float distanceNeeded = 3f;
    private float stayTimeRequired;
    private float currentStayTime = 0f;
    private GameObject effect;
    private bool isPlayerInside = false;

    public QuestReachPoint(string name, List<Transform> players, Transform target, GameObject effectPrefab, float stayTime = 5f) : base(name)
    {
        this.questName = name;
        this.players = players;
        this.target = target;
        this.stayTimeRequired = stayTime;
        this.effect = effectPrefab;
        this.questText = "Nhiệm vụ: " + name;
    }

    public override void StartQuest()
    {
        base.StartQuest();
        currentStayTime = 0f;
        isPlayerInside = false;
    }

    public override void UpdateQuest()
    {
        if (isCompleted || target == null) return;

        bool anyPlayerInside = false;
        foreach (var p in players)
        {
            if (p != null && Vector3.Distance(p.position, target.position) <= distanceNeeded)
            {
                anyPlayerInside = true;
                break;
            }
        }

        if (anyPlayerInside)
        {
            isPlayerInside = true;
            currentStayTime += Time.deltaTime;

            // Cập nhật đếm ngược lên UI
            float remain = Mathf.Max(0, stayTimeRequired - currentStayTime);
            GameEvents.OnQuestTextChanged?.Invoke($"{questText} ({remain:F1}s)");

            if (currentStayTime >= stayTimeRequired) CompleteQuest();
        }
        else if (isPlayerInside)
        {
            // RESET khi người chơi rời khỏi vùng
            isPlayerInside = false;
            currentStayTime = 0f;
            GameEvents.OnQuestTextChanged?.Invoke(questText);
        }
    }

    public override void CompleteQuest()
    {
        isCompleted = true;
        if (effect != null)
        {
            GameObject fx = Object.Instantiate(effect, target.position, Quaternion.identity);
            Object.Destroy(fx, 10f);
        }
    }
}