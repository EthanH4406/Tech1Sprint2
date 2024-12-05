using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunStats
{
    public float fireRateCooldown;
    public int maxAmmoCountPerMag;
    public float reloadCoolDown;
    public float weaponDura; //number 1-5;



    public void TakeDamage()
    {
        weaponDura -= 1;
        weaponDura = Mathf.Clamp(weaponDura, 1, 6);

        
    }
}
