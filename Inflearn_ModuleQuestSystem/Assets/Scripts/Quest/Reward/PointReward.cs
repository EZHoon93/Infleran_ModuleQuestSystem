using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Reward/Point" , fileName = "PointReward_") ]
public class PointReward : Reward
{
    public override void Give(Quest quest)
    {
        GameSystem.Instance.AddScore(Quantity);
        Debug.Log(Quantity);
        PlayerPrefs.SetInt("boundScore", Quantity);
        PlayerPrefs.Save();
    }
}
