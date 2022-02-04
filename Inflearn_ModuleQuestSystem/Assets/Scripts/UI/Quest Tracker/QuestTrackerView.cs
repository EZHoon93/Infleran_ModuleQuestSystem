using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class QuestTrackerView : MonoBehaviour
{
    [SerializeField]
    private QuestTracker questTrackerPrefab;
    [SerializeField]
    private CategoryColor[] categoryColors;

    //QuesySystem을통해 새로운이벤트가생성되 새로운 QuestTracker가만들어지게
    private void Start()
    {
        QuestSystem.Instance.onQuestRegistered += CreateQuestTeacker;

        foreach (var quest in QuestSystem.Instance.ActiveQuests)
            CreateQuestTeacker(quest);
    }
    private void OnDestroy()
    {
        if (QuestSystem.Instance)
            QuestSystem.Instance.onQuestRegistered -= CreateQuestTeacker;
    }

    private void CreateQuestTeacker(Quest quest)
    {
        var categoryColor = categoryColors.FirstOrDefault(x => x.category == quest.Category);
        var color = categoryColor.category == null ? Color.white : categoryColor.color;
        Instantiate(questTrackerPrefab, transform).Setup(quest, color);

    }

    [System.Serializable]
    private struct CategoryColor
    {
        public Category category;
        public Color color;
    }
}


