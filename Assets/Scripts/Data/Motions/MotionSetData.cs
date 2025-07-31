using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newMotionSetData", menuName = "Data/Motion Set Data/Base Data")]
public class MotionSetData : ScriptableObject
{
    public MotionData[] motions;
}
