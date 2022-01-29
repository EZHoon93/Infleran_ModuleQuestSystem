using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Task/Action/PostiveCount", fileName = "PostiveCount")]

public class PostiveCount : TaskAction
{
    public override int Run(Task task, int currentSucess, int sucessCount)
    {
        return sucessCount > 0 ? currentSucess + sucessCount : sucessCount;
    }
}
