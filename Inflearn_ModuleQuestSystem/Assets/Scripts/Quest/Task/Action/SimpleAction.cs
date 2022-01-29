using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Task/Action/SimpleCount", fileName = "Simple Count")]

public class SimpleAction : TaskAction
{
    public override int Run(Task task, int currentSucess, int sucessCount)
    {
        return currentSucess + sucessCount;
    }
}
