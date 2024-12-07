using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAmmoCounter : MonoBehaviour
{

    public int ammoCount;

    public Image[] bulletImages;

    private GameManager gameManager;

    private int maxIndex;
    private int magCount;
    private int maxAmmoCount;
    private float delay = 0.1f;
    private float timer;

	private void Awake()
	{
		gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
	}

	private void Start()
	{
        timer = Time.time;

        ammoCount = bulletImages.Length;

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

	public void AddAmmo(int amo)
    {
        ammoCount += amo;
        ammoCount = Mathf.Clamp(ammoCount, 0, maxAmmoCount);

        
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

				//spawn bullet
				GameObject bullet = gameManager.objPooler.SpawnFromPool("player_bullet", this.gameObject.transform.position, Quaternion.identity);
				bullet.SetActive(true);
				bullet.GetComponent<PlayerBullet>().Launch(Vector2.left, 5f);
			}
			else
			{
				foreach (Image b in bulletImages)
				{
					b.enabled = false;
				}
			}

            timer = Time.time;
		}

        
    }

}


