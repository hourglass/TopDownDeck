using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newActionData", menuName = "Data/Action Data/Base Data")]
public class ActionData : ScriptableObject
{
    public MotionData[] motions;
}