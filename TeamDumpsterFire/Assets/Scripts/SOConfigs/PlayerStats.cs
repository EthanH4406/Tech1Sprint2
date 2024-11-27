using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Parameters", menuName = "Presets/Player")]
public class PlayerStats : ScriptableObject
{
    public float walkSpeed;
    public float health;
    public float regenAmount;
    public float regenCooldown;
    public float iFrameDuration;

    
}
