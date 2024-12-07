using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashFishingBehaviour : MonoBehaviour
{
    public AssetDescripter[] potentialDrops;
    public int numberOfItemAttempts;

    private CircleCollider2D interactCollider;
    private bool canPop;
    private GameObject player;
    private Vector2 playerPos;
    private bool activate = false;
    private bool cancelOut = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        interactCollider = this.gameObject.GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = player.transform.position;

        if(interactCollider.bounds.Contains(playerPos))
        {
            canPop = true;
        }
        else
        {
            canPop = false;
        }

        if(activate && canPop && !cancelOut)
        {
            StartCoroutine("Fish");
        }
    }

    public void Interact()
    {
        activate = true;
    }

    IEnumerator Fish()
    {
        cancelOut = true;

        if(canPop)
        {
            for(int i=0; i < numberOfItemAttempts; i++)
            {
                int itemIndex = Random.Range(0, potentialDrops.Length);
                int rng = Random.Range(0, 101);

                if (potentialDrops[itemIndex].spawnChance <= rng)
                {
                    Instantiate(potentialDrops[itemIndex], this.gameObject.transform.position, Quaternion.identity);
                    yield return new WaitForSeconds(0.5f);
                }
            }

            Destroy(this.gameObject);
        }
    }
}
