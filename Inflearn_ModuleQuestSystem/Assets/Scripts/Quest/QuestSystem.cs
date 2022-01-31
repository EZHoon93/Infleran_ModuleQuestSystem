using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class QuestSystem : MonoBehaviour
{
    #region Event
    public delegate void QuestRegisterHandler(Quest newQuest);
    public delegate void QuestCompletedHandler(Quest newQuest);
    public delegate void QuestCancelHandler(Quest newQuest);


    #endregion
    private static QuestSystem instance;
    private static bool isApplicationQuitting;

    public static QuestSystem Instance
    {
        get
        {
            if(!isApplicationQuitting && instance == null)
            {
                instance = FindObjectOfType<QuestSystem>();
                if(instance == null)
                {
                    instance = new GameObject("Quest System").AddComponent<QuestSystem>();
                    DontDestroyOnLoad(instance.gameObject); 
                }
            }
            return instance;
        }
    }

    private List<Quest> aciveQuests = new List<Quest>();
    private List<Quest> completedQuests = new List<Quest>();
    private List<Quest> activeAchivements = new List<Quest>();
    private List<Quest> completedAchievements = new List<Quest>();
    private QuestDatabase questDatabase;
    private QuestDatabase achievementDatabase;

    public event QuestRegisterHandler onQuestRegistered;
    public event QuestCompletedHandler onQuestCompleted;
    public event QuestCancelHandler onQuestCanceled;

    public event QuestRegisterHandler onAchievementRegistered;
    public event QuestCompletedHandler onAchievementCompleted;

    public IReadOnlyCollection<Quest> ActiveQuests => aciveQuests;
    public IReadOnlyCollection<Quest> CompletedQuests => completedQuests;
    public IReadOnlyCollection<Quest> ActiveAchievements => activeAchivements;
    public IReadOnlyCollection<Quest> CompletedAchievements=> completedAchievements;

    private void Awake()
    {
        questDatabase = Resources.Load<QuestDatabase>("QuestDatabase");
        achievementDatabase = Resources.Load<QuestDatabase>("AchievementDatabase");

        foreach(var achievement in achievementDatabase.Quests)
        {
            Register(achievement);
        }
    }
    /// <summary>
    /// 강한결합은 좋지않음, 클래스에서 복사본을 얻어옴
    /// </summary>
    public Quest Register(Quest quest)
    {
        var newQuest = quest.Clone();

        if(newQuest is Achievement)
        {
            newQuest.onCompleted += OnAchievementCompleted;
            activeAchivements.Add(newQuest);
            newQuest.OnRegister();
            onAchievementRegistered?.Invoke(newQuest);
        }
        else
        {
            newQuest.onCompleted += OnQuestCompleted;
            newQuest.onCompleted += OnQuestCanceled;
            aciveQuests.Add(newQuest);
            newQuest.OnRegister();
            onQuestRegistered?.Invoke(newQuest);
        }

        return newQuest;
    }

    public void ReceiveReport(string category, object target, int successCount)
    {
        print($"{category} / {target}");
        ReceiveReport(aciveQuests, category, target, successCount);
        ReceiveReport(activeAchivements, category, target, successCount);

    }
    public void ReceiveReport(Category category, TaskTarget target, int successCount)
    {
        ReceiveReport(category.CodeName, target.Value, successCount);
    }
    //내부에서사용
    private void ReceiveReport(List<Quest> quests , string category , object target , int successCount)
    {
        foreach (var quest in quests)
            quest.ReceiveReport(category, target, successCount);
    }
    //Quest가 목록에있는지 확인
    public bool ContainsInActiveQuests(Quest quest)
    {
        return ActiveQuests.Any(x => x.CodeName == quest.CodeName);
    }
    public bool ContainsInCompleteQuests(Quest quest)
    {
        return completedQuests.Any(x => x.CodeName == quest.CodeName);
    }
    public bool ContainsInActiveAchievements(Quest quest)
    {
        return activeAchivements.Any(x => x.CodeName == quest.CodeName);
    }
    public bool ContainsInCompletedAchievements(Quest quest)
    {
        return completedAchievements.Any(x => x.CodeName == quest.CodeName);
    }

    #region CallBack
    private void OnQuestCompleted(Quest quest)
    {
        aciveQuests.Remove(quest);
        completedQuests.Add(quest);

        onQuestCompleted?.Invoke(quest);
    }

    private void OnQuestCanceled(Quest quest)
    {
        aciveQuests.Remove(quest);
        onQuestCanceled?.Invoke(quest);

        Destroy(quest, Time.deltaTime);
    }

    private void OnAchievementCompleted(Quest achievement)
    {
        activeAchivements.Remove(achievement);
        completedAchievements.Add(achievement);

        onAchievementCompleted?.Invoke(achievement);
    }
    #endregion

}
