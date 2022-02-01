using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum TaskState
{
    Inactive,
    Running,
    Complete
}
[CreateAssetMenu(menuName = "Quest/Task/Task", fileName = "Task_")]

public class Task : ScriptableObject
{
    #region Events
    public delegate void StateChangeHandler(Task task, TaskState currentState, TaskState prevState);
    public delegate void SucessChangeHandler(Task task, int currentSuccess, int prevSuccess);
    #endregion
    [SerializeField]
    Category category;
    [Header("Text")]
    [SerializeField]
    private string codeName;
    [SerializeField]
    private string description;


    [Header("Action")]
    [SerializeField]
    private TaskAction action;

    [Header("Target")]
    [SerializeField]
    private TaskTarget[] targets;

    [Header("Setting")]
    [SerializeField]
    private InitializeSuccessValue initializeSuccessValue;
    [SerializeField]
    private int needSucessToComplete;
    [SerializeField]
    private bool canReceiveReportDuringCompletion;  //Task가 완료되어도 계속 성공횟수를 갖고잇는지

    private TaskState state;
    private int currentSuccess;
    public StateChangeHandler onStateChanged;
    public SucessChangeHandler onSuccessChanged;

    public Category Category => category;

    public int CurrentSuccess
    {
        get => currentSuccess;
        set
        {
            var prevSucess = currentSuccess;
            currentSuccess = Mathf.Clamp(value ,0 , needSucessToComplete);
            if(currentSuccess != prevSucess)
            {
                State = currentSuccess == needSucessToComplete ? TaskState.Complete : TaskState.Running;
                onSuccessChanged?.Invoke(this, currentSuccess, prevSucess);

            }
        }
    }
    
    public string CodeName => codeName;

    public string Description => description;

    public int NeedSucessToComplete => needSucessToComplete;

    public TaskState State
    {
        get => state;
        set
        {
            var prevState = state;
            state = value;
            onStateChanged?.Invoke(this, state, prevState);

        }
    }

    public bool IsComplete => State == TaskState.Complete;
    public Quest Owner { get; private set; }

    public void Setup(Quest owner)
    {
        Owner = owner;
    }

    public void Start()
    {
        State = TaskState.Running;
        if (initializeSuccessValue)
        {
            CurrentSuccess = initializeSuccessValue.GetValue(this);
        }
    }

    public void End()
    {
        onStateChanged = null;
        onStateChanged = null;
    }

    public void ReceiveReport(int successCount)
    {
        CurrentSuccess = action.Run(this, CurrentSuccess, successCount);    //가능하다면 누가 호출햇는지를위해 this도 보
    }

    public void Complete()
    {
        CurrentSuccess = needSucessToComplete;
    }

    public bool IsTarget(string category , object target)
        => Category == category && targets.Any(x => x.IsEqul(target)) && (!IsComplete || (IsComplete && canReceiveReportDuringCompletion));
}
