using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

using Debug = UnityEngine.Debug;
using UnityEditor;
using System.Linq;
using System.Xml;

public enum QuestState
{
    Inactive,
    Running,
    Complete,
    Cancel,
    WatingForCompletetion
}
[CreateAssetMenu( menuName = "Quest/Quest" , fileName = "Quest_")]
public class Quest : ScriptableObject
{
    #region Event
    public delegate void TaskSucessChangeHandler(Quest quest, Task task, int currentSuccess, int prevSuccess);
    public delegate void CompleteHandler(Quest quest);
    public delegate void CancelHandler(Quest quest);
    public delegate void NewTaskGroupHandler(Quest quest , TaskGroup currentTaskGroup , TaskGroup prevTaskGroup);

    #endregion

    [SerializeField]
    private Category category;
    [SerializeField]
    private Sprite icon;

    [Header("Text")]
    [SerializeField]
    private string codeName;
    [SerializeField]
    private string displayName;
    [SerializeField, TextArea]
    private string description;

    [Header("Task")]
    [SerializeField]
    private TaskGroup[] taskGroups;

    [Header("Reward")]
    private Reward[] rewards;
    [Header("Option")]
    [SerializeField]
    private bool useAutoComplete;
    [SerializeField]
    private bool  isCancelable;

    [Header("Condition")]
    [SerializeField]
    private Condition[] acceptionConditions;
    [SerializeField]
    private Condition[] cancelConditions;
    private int currentTaskGroupIndex;

    public Category Category => category;
    public Sprite Icon => icon;
    public string CodeName => codeName;
    public string DiplayName => displayName;
    public string Description => description;

    public QuestState State { get; private set; }
    public TaskGroup CurrentTaskGroup => taskGroups[currentTaskGroupIndex];
    public IReadOnlyList<TaskGroup> TaskGroups => taskGroups;
    public IReadOnlyList<Reward> Rewards => rewards;
    public bool IsRegistered => State != QuestState.Inactive;
    public bool IsComplatable => State == QuestState.WatingForCompletetion;
    public bool IsComplete => State == QuestState.Complete;
    public bool IsCancel => State == QuestState.Cancel;
    public virtual bool IsCancelable => isCancelable && cancelConditions.All(x => x.IsPass(this));
    public bool IsAcceptable => acceptionConditions.All(x => x.IsPass(this));
    public event TaskSucessChangeHandler onTaskSuccessChanged;
    public event CompleteHandler onCompleted;
    public event CancelHandler onCanceled;
    public event NewTaskGroupHandler onNewTaskGroup;

    public void OnRegister()
    {
        Debug.Assert(!IsRegistered, "This Quest has alreaday been registed");
        foreach(var taskgroup in taskGroups)
        {
            taskgroup.Setup(this);
            foreach (var task in taskgroup.Tasks)
                task.onSuccesChanged += OnSuccessChanged;
        }

        State = QuestState.Running;
        currentTaskGroupIndex = 0;
        Debug.Log(currentTaskGroupIndex +"/"+TaskGroups.Count);
        CurrentTaskGroup.Start();
    }

    public void ReceiveReport(string category, object target, int sucessCount)
    {
        CheckIsRunning();
        if (IsComplete)
            return;

        if (CurrentTaskGroup.IsAllTaskComplete)
        {
            if(currentTaskGroupIndex +1 == taskGroups.Length)
            {
                State = QuestState.WatingForCompletetion;
                if (useAutoComplete)
                    Complete();
            }
            else
            {
                var prevTaskGroup = taskGroups[currentTaskGroupIndex++];
                prevTaskGroup.End();
                CurrentTaskGroup.Start();
                onNewTaskGroup?.Invoke(this, CurrentTaskGroup, prevTaskGroup);
            }
        }
        else
        {
            State = QuestState.Running; //완료가되었어도 계속보고를받아야하는 옵션이있엇음, 안깨질수도있으므로 Running상태로변경
        }

    }

    public void Complete()
    {
        CheckIsRunning();

        foreach (var taskgroup in taskGroups)
            taskgroup.Complete();

        State = QuestState.Complete;

        foreach (var reward in rewards)
            reward.Give(this);

        onCompleted?.Invoke(this);

        onTaskSuccessChanged = null;
        onCompleted = null;
        onCanceled = null;
        onNewTaskGroup = null;

        // 보
    }

    public virtual void Cancel()
    {
        CheckIsRunning();
        Debug.Assert(IsCancelable, "Thi Quest can't be cancled");
        State = QuestState.Cancel;
        onCanceled?.Invoke(this);

    }

    public Quest Clone()
    {
        var clone = Instantiate(this);
        clone.taskGroups = taskGroups.Select(x => new TaskGroup(x)).ToArray();

        return clone;
    }

    private void OnSuccessChanged(Task task , int currentSuccess, int prevSuccess)
    {
        onTaskSuccessChanged?.Invoke(this, task, currentSuccess, prevSuccess);
    }


#if UNITY_EDITOR

#endif
    [Conditional("UNITY_EDITOR")]
    private void CheckIsRunning()
    {
        Debug.Assert(IsRegistered, "This Quest has alreaday been registed");
        Debug.Assert(!IsCancel, "This Quest has been Canceled");
        Debug.Assert(!IsComplatable, "This Quest has been Complete");
    }
    //슬라임 10마리 잡아라..!!
    //깨고나면 레드슬라임 10마리잡아라..!!

    //=> 하나의 퀘스트에 여러개 테스크가있을수있다.
}
