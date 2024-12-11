using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashFishingBehaviour : MonoBehaviour
{
    public Sprite[] sprites;
    public List<GameObject> spawnableItems;

    private SpriteRenderer spriteRenderer;

	private void Awake()
	{
		spriteRenderer = this.GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
	}

	public void SpawnItem()
	{
		int chance = Random.Range(0, spawnableItems.Count);
		Instantiate(spawnableItems[chance], this.transform.position, Quaternion.identity);

		Destroy(this.gameObject);
		
	}
}
