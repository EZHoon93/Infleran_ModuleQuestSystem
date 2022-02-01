using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Acievement", fileName ="Achivement_")]
public class Achievement : Quest
{
    public override bool IsCancelable => false;

    public override bool IsSavable => true;

    public override void Cancel()
    {
        Debug.LogAssertion("Acievement can't be canceled");
    }


}
