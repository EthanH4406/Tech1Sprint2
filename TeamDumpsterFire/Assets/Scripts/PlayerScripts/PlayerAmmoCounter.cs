using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAmmoCounter : MonoBehaviour
{
    //this script handles the ammo UI and management for the player
    private List<AmmoType> ammoTypes = new List<AmmoType>();
    

    public void AddAmmo(string type, int amo)
    {
        foreach(AmmoType ammo in ammoTypes)
        {
            if(ammo.type.Equals(type))
            {
                ammo.count += amo;
                return;
            }
        }
    }

    //returns a bool if the gun still has ammo
    public bool ConsumeAmmo(string type, int amo)
    {
        foreach (AmmoType ammo in ammoTypes)
        {
            if (ammo.type.Equals(type))
            {
                ammo.count -= amo;

                if(ammo.count <= 0)
                {
                    ammo.count = 0;
                    return false;
                }

                break;
            }
        }

        return true;
    }

}

public class AmmoType
{
    public string type;
    public int count;
    public float damage;

    public AmmoType(string type, int count, float damage)
    {
        this.type = type;
        this.count = count;
        this.damage = damage;
    }
}
