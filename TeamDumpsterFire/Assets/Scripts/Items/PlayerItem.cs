using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItem
{
    public string name;
    public int id;
    public int count;
    public bool consume;
    public GameObject itemPickup;

    PlayerItem(string name, int id, int count, bool consume, GameObject itemPhysc)
    {
        this.name = name;
        this.id = id;
        this.count = count;
        this.consume = consume;
        this.itemPickup = itemPhysc;
    }
}
