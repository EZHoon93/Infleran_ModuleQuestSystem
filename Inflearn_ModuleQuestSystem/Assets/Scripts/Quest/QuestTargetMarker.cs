using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class QuestTargetMarker : MonoBehaviour
{
    [SerializeField]
    private TaskTarget target;
    [SerializeField]
    private MarkerMaterialData[] markerMaterialDatas;

    private Dictionary<Quest, Task> targetTaskByQuest = new Dictionary<Quest, Task>();


    private Transform cameraTransform;
    private Renderer renderer;

    private int currentRunningTargetTaskCount;

    [System.Serializable]
    private struct MarkerMaterialData
    {
        public Category category;
        public Material markerMaterial;
    }
    private void Awake()
    {
        cameraTransform = Camera.main.transform;
        renderer = GetComponent<Renderer>();
    }
    private void Start()
    {
        gameObject.SetActive(false);

        QuestSystem.Instance.onQuestRegistered += TryAddTargetQuest;
        foreach (var quest in QuestSystem.Instance.ActiveQuests)
            TryAddTargetQuest(quest);

    }

    private void Update()
    {
        var rotation = Quaternion.LookRotation((cameraTransform.position - transform.position).normalized);
        transform.rotation = Quaternion.Euler(0f, rotation.eulerAngles.y + 180f, 0f);
    }
    private void OnDestroy()
    {
        QuestSystem.Instance.onQuestRegistered -= TryAddTargetQuest;
        foreach ( var quest in targetTaskByQuest.Keys)
        {
            quest.onNewTaskGroup -= UpdateTargetTask;
            quest.onCompleted -= RemoveTargetQuest;
        }
        foreach(var task in targetTaskByQuest.Values)
        {
            task.onStateChanged -= UpdateRunningTargetTaskCount;
        }

        //foreach ( (Quest quest ,Task task) in targetTaskByQuest)
        //{

        //}
    }
    private void TryAddTargetQuest(Quest quest)
    {
        if(target != null && quest.ContainsTarget(target))
        {
            quest.onNewTaskGroup += UpdateTargetTask;
            quest.onCompleted += RemoveTargetQuest;

            UpdateTargetTask(quest, quest.CurrentTaskGroup);
        }
    }

    private void UpdateTargetTask(Quest quest, TaskGroup currentTaskGroup, TaskGroup prevTaskGroup = null)
    {
        targetTaskByQuest.Remove(quest);

        var task = currentTaskGroup.FindTaskByTarget(target);
        if(task != null)
        {
            targetTaskByQuest[quest] = task;
            task.onStateChanged += UpdateRunningTargetTaskCount;
            UpdateRunningTargetTaskCount(task, task.State);
        }
    }

    private void RemoveTargetQuest(Quest quest) => targetTaskByQuest.Remove(quest);


    private void UpdateRunningTargetTaskCount(Task task, TaskState currentState, TaskState prevState = TaskState.Inactive)
    {
        if (currentState == TaskState.Running)
        {
            renderer.material = markerMaterialDatas.First(x => x.category == task.Category).markerMaterial;
            currentRunningTargetTaskCount++;
        }
        else
            currentRunningTargetTaskCount--;

        gameObject.SetActive(currentRunningTargetTaskCount != 0);

    }
}
