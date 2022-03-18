using System.Collections.Generic;
using Gamee_Hiukka.Daily;
using UnityEngine;

[CreateAssetMenu(fileName = "WeekRewardData", menuName = "Reward Data/Week Reward Data", order = 0)]
public class WeekRewardData : ScriptableObject
{
    public List<Reward> rewards;
}
