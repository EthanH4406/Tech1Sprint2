using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public PlayerStats stats;

    private PlayerMovement movementDriver;
    private List<GenericBuff> currentBuffs = new List<GenericBuff>();

    private void Awake()
    {
        movementDriver = GetComponent<PlayerMovement>();
    }

    public void GivePlayerBuff(int buffId)
    {
        //get a buff from id
        //add it to currentBuffs
    }
}
