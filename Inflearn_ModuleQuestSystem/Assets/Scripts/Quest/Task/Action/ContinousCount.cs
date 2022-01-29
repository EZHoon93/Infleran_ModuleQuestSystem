using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Task/Action/ContinousCount", fileName = "ContinousCount")]

public class ContinousCount : TaskAction
{
    public override int Run(Task task, int currentSucess, int sucessCount)
    {
        return sucessCount > 0 ? currentSucess + sucessCount : 0;
    }
}
