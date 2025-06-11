using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Move State")]
    public float movementVelocity = 4f;

    [Header("Dash State")]
    public int amountOfDashs = 2;
    public float dashForce = 12f;
    public float dashDrag = 5f;
    public float dashTime = 0.4f;
    public float dashCoolDown = 1f;
    //public float distanceOfAfterImage = 0.5f;
}
