using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    public string name;
    public int id;
    public string type;
    public int count;
    public bool consume;
    public GameObject itemPickup;

    PlayerItem(string name, int id, string type, int count, bool consume, GameObject itemPhysc)
    {
        this.name = name;
        this.id = id;
        this.type = type;
        this.count = count;
        this.consume = consume;
        this.itemPickup = itemPhysc;
    }
}
