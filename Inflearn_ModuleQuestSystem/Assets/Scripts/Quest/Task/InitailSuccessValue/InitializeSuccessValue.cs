using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InitializeSuccessValue : ScriptableObject
{
    public abstract int GetValue(Task task);
}
