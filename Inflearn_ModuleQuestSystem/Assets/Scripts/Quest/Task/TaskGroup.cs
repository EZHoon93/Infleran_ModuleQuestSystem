﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public enum TaskGroupState
{
    Inactive,
    Running,
    Complete
}

[System.Serializable]
public class TaskGroup 
{
    [SerializeField]
    private Task[] tasks;

    public IReadOnlyList<Task> Tasks => tasks;

    public Quest Owner { get; private set; }

    public bool IsAllTaskComplete => tasks.All(x => x.IsComplete);
    public bool IsComplete => State == TaskGroupState.Complete;

    public TaskGroupState State { get; private set; }

    public void Setup(Quest owner)
    {
        Owner = owner;
        foreach (var task in tasks)
            task.Setup(owner);
    }


    //현재 작동해야하는 테스크들이 시작할것을찾고 시작.
    public void Start()
    {
        State = TaskGroupState.Running;
        foreach(var task in tasks)
        {
            task.Start();
        }
    }

    public void End()
    {
        foreach (var task in tasks)
            task.End();
    }

    //Task 가 해당 카테고리와 타겟을 가지고있으면 보고받음

    public void ReceiveReport(string category , object target , int successCount)
    {
        foreach(var task in tasks)
        {
            if(task.IsTarget(category, target))
            {
                task.ReceiveReport(successCount);
            }
        }
    }

    public void Complete()
    {
        if (IsComplete)
            return;

        State = TaskGroupState.Running;
        foreach(var task in tasks)
        {
            if (!task.IsComplete)
                task.Complete();
        }
    }

}