using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAmmoCounter : MonoBehaviour
{

    public int ammoCount;

    public void AddAmmo(int amo)
    {
        ammoCount += amo;
    }

    //returns the number of bullets consumed. If the ammount goes negative, it gets clamped and the actual amount gets returned. Then compare to see if they match.
    public int ConsumeAmmo(int amo)
    {
        int oldAmount = ammoCount;
        ammoCount -= amo;

        ammoCount = Mathf.Clamp(ammoCount, 0, 3000);
        return oldAmount - ammoCount;
    }

}


