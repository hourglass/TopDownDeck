using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newMotionData", menuName = "Data/Motion Data/Base Data")]
public class MotionData : ScriptableObject
{
    public AnimationClip[] animations;
}

