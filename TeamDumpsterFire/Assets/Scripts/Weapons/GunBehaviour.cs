using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBehaviour : MonoBehaviour
{
    public int currentAmmoCount;
    public int maxAmmoCount;
    public int magCount;

    public float shotCoolDown;

    PlayerAmmoCounter ammoCounter;


	private void Awake()
	{
		ammoCounter = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAmmoCounter>();
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
