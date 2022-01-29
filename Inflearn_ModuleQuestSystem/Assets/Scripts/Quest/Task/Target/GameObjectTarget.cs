using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Task/Target/GameObjectTarget", fileName = "Target_")]

public class GameObjectTarget : TaskTarget
{
    [SerializeField]
    private GameObject value;

    public override object Value => value;

    public override bool IsEqul(object target)
    {
        var targetAsGameObject = target as GameObject;

        if (targetAsGameObject == null)
            return false;


        return targetAsGameObject.name.Contains(value.name);
    }
}
