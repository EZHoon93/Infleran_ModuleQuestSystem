using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

public class QuestTracker : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI questTitleText;
    [SerializeField]
    private TaskDescriptor taskDescriptorPrefab;

    private Dictionary<Task, TaskDescriptor> taskDescriptorByTask = new Dictionary<Task, TaskDescriptor>();

    private Quest targetQuest;

    private void OnDestroy()
    {
        if(targetQuest != null)
        {
            targetQuest.onNewTaskGroup -= UpdateTaskDescriptors;
            targetQuest.onCompleted -= DestroySelf;
        }

        foreach(var tuple in taskDescriptorByTask)
        {
            var task = tuple.Key;
            task.onSuccessChanged -= UpdateText;
        }
    }
    public void Setup(Quest targetQuest , Color titleColor)
    {
        this.targetQuest = targetQuest;
        questTitleText.text = targetQuest.Category == null ? targetQuest.DisplayName : $"[{targetQuest.Category.DisplayName}] {targetQuest.DisplayName}";
        questTitleText.color = titleColor;
        targetQuest.onNewTaskGroup += UpdateTaskDescriptors;
        targetQuest.onCompleted += DestroySelf;

        var taskGroups = targetQuest.TaskGroups;
        UpdateTaskDescriptors(targetQuest, taskGroups[0]);

        //Save된상태를 Load해왓을때 앞에 Task가 이미 깬상태일수있으므로
        if(taskGroups[0] != targetQuest.CurrentTaskGroup)
        {
            for(int i = 1; i < taskGroups.Count; i++)
            {
                var taskGroup = taskGroups[i];
                UpdateTaskDescriptors(targetQuest, taskGroup, taskGroups[i - 1]);
                if (taskGroup == targetQuest.CurrentTaskGroup)
                    break;
            }
        }
    }



    private void UpdateTaskDescriptors(Quest quest , TaskGroup currentTaskGroup , TaskGroup prevTaskGroup = null)
    {
        foreach(var task in currentTaskGroup.Tasks)
        {
            var taskDescriptor = Instantiate(taskDescriptorPrefab , transform);
            taskDescriptor.UpdateText(task);
            task.onSuccessChanged += UpdateText;

            taskDescriptorByTask.Add(task, taskDescriptor);
        }

        if(prevTaskGroup != null)
        {
            foreach(var task in prevTaskGroup.Tasks)
            {
                var taskDescriptor = taskDescriptorByTask[task];
                taskDescriptor.UpdateTextUsingStrikeThrought(task);
            }
        }
    }

    private void UpdateText(Task task , int currentSuccess, int prevSuccess)
    {
        taskDescriptorByTask[task].UpdateText(task);
    }

    private void DestroySelf(Quest quest)
    {
        Destroy(gameObject);
    }
}
