using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Move State")]
    public float movementVelocity = 4f;

    [Header("Roll State")]
    public int amountOfRolls = 2;
    public float rollForce = 12f;
    public float rollDrag = 5f;
    public float rollTime = 0.25f;
    public float rollCoolDown = 0.5f;
}
