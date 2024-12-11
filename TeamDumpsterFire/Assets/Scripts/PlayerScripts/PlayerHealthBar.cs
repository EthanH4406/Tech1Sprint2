using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    private int maxHP = 4;
    private int currentHP;

    public Sprite[] sprites;
    //public GameObject healthBarImage;


    public Image hpBar;
    private Sprite activeSprite;
    private int currentSpriteIndex;


    //temp
    float timer;
    float delay = 5f;

	

	private void Start()
	{
        ResetHealth();
        
	}

	private void Awake()
	{
		//hpBar = healthBarImage.GetComponent<Image>();
	}

	public void ResetHealth()
    {
        currentSpriteIndex = sprites.Length - 1;
        activeSprite = sprites[currentSpriteIndex];
        hpBar.sprite = activeSprite;
        currentHP = maxHP;
    }

    public void TakeDamage()
    {
        currentHP -= 1;

		//Debug.Log("Current health: " + currentHP);
		//Debug.Log("Current sprite index: " + currentSpriteIndex);

        if(currentHP <= 0)
        {
            activeSprite = sprites[0];
            hpBar.sprite = activeSprite;
            Die();
            return;
        }

        currentSpriteIndex--;
        activeSprite = sprites[currentSpriteIndex];
        hpBar.sprite = activeSprite;
    }

    public void Heal()
    {
        currentHP++;
        currentSpriteIndex++;

        if(currentHP >= maxHP)
        {
            currentHP = maxHP;
            currentSpriteIndex = sprites.Length - 1;
        }

        
        activeSprite = sprites[currentSpriteIndex];
        hpBar.sprite = activeSprite;
    }

    private void Die()
    {
        Debug.Log("Player has died");
        //Application.Quit();
    }

}
