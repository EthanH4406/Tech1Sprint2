using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleporterBoss : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag("Player"))
		{
			GameObject.FindGameObjectWithTag("transitionPanel").GetComponent<Animator>().SetTrigger("FadeOut");
			StartCoroutine(LoadEndGame());
		}

		
	}

	IEnumerator LoadEndGame()
	{
		yield return new WaitForSeconds(1.5f);

		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(3);

		while (!asyncLoad.isDone)
		{
			yield return null;
		}
	}
}
