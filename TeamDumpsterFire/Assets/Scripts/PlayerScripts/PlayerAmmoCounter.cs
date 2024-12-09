using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAmmoCounter : MonoBehaviour
{

    [Header("Debug")]
    public bool enableAmmoReadout;

    public int ammoCount;

    public Image[] bulletImages;

    private GameManager gameManager;

    private int maxIndex;
    public int magCount;
    private int maxAmmoCount;
    private float timer;
    private Vector2 mousePos;

    private int healthStage;
    private float chanceToTakeDamage = 3;

    private float bulletDmg;
    private float maxBulletDmg;
    private float minBulletDmg;
    public float initialDmg = 7;

	private float delay = 0.1f;
    private float minDelay;
    private float maxDelay;
    public float initialDelay = 3;


	private void Awake()
	{
		gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
	}

	private void Start()
	{
        timer = Time.time;

        delay = initialDelay;
        bulletDmg = initialDmg;

        ammoCount = bulletImages.Length;
        magCount = bulletImages.Length;

        maxIndex = bulletImages.Length - 1;

        //as bullets consume, subtract 1 from index


        foreach(Image b in bulletImages)
        {
            b.enabled = false;
        }

        for(int i=0; i<ammoCount; i++)
        {
            bulletImages[i].enabled = true;
        }
	}

	private void Update()
	{
		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}

	public void AddAmmo(int amo)
    {
        ammoCount += amo;
        ammoCount = Mathf.Clamp(ammoCount, 0, magCount);

        if(enableAmmoReadout)
        {
            Debug.Log("Ammo has been reloaded: " + ammoCount + " / " + magCount);
        }
        


        for(int i=0; i < ammoCount; i++)
        {
            bulletImages[i].enabled=true;
        }
    }

    //returns the number of bullets consumed. If the ammount goes negative, it gets clamped and the actual amount gets returned. Then compare to see if they match.
    public void ShootBullet()
    {
        if(Time.time > timer + delay)
        {
			if (ammoCount > 0)
			{
				bulletImages[ammoCount - 1].enabled = false;
				ammoCount--;

                float offset = 0.5f;
                Vector2 mouseDirc = (mousePos - (Vector2)this.transform.position).normalized;
                Vector3 bulletSpawnPos = new Vector3(mouseDirc.x * offset, mouseDirc.y * offset, 0);


                Debug.Log("Bullet Stats before shot. Delay: " + delay + ". Damage: " + bulletDmg);


				//spawn bullet
				GameObject bullet = gameManager.objPooler.SpawnFromPool("player_bullet", this.gameObject.transform.position + bulletSpawnPos, Quaternion.identity);
				bullet.GetComponent<PlayerBullet>().Launch(mouseDirc, 25f);

                int rng = Random.Range(0, 101);
                if(rng <= chanceToTakeDamage)
                {
                    DamageWeapon();
                    
                }
			}
			else
			{
				foreach (Image b in bulletImages)
				{
					b.enabled = false;
				}
			}

            if(enableAmmoReadout)
            {
                Debug.Log("Current Ammo: " + ammoCount + " / " + magCount);
            }

			

			timer = Time.time;
		}

        
    }

    private void DamageWeapon()
    {
        delay += 0.1f;
        bulletDmg -= 1;

        if( bulletDmg <= 1)
        {
            bulletDmg = 1;
        }

    }

   

    public void Repair()
    {
        delay = initialDelay;
        bulletDmg = initialDmg;
        healthStage = 5;
    }

    public void Reload()
    {
        AddAmmo(magCount);
    }

    public void IncreaseAmmoCount()
    {
        magCount++;
        magCount = Mathf.Clamp(magCount, 0, maxAmmoCount);
    }

    public void IncreaseFirerate()
    {
        delay -= 0.1f;
        delay = Mathf.Clamp(delay, 0.1f, 5000f);

    }


}


