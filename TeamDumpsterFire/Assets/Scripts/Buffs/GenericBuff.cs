using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericBuff : MonoBehaviour
{
    public float duration;
    public int id;

    private float timer;

    private void Start()
    {
        timer = Time.time;
    }

    private void Update()
    {
        if(Time.time >= timer + duration)
        {
            //remove buff from player
        }
    }

}
