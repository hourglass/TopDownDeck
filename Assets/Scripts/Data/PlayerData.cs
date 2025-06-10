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
    public float dashVelocity = 10f;
    public float dashForce = 12f;
    public float dashDrag = 5f;
    public float dashTime = 0.3f;
    public float singleDashCooldown = 0.03f;
    public float dashCoolDown = 0.3f;
    //public float distanceOfAfterImage = 0.5f;
}
