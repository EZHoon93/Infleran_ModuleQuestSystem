using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Task/Action/SimpleSet", fileName = "Simple Set")]

public class SimpleSet : TaskAction
{
    public override int Run(Task task, int currentSucess, int sucessCount)
    {
        return sucessCount;
    }
}
