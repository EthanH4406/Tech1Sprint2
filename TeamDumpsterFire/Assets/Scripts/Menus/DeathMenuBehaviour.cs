using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenuBehaviour : MonoBehaviour
{
	public Animator animator;

	public void ReturnToMenu()
	{
		animator.SetTrigger("FadeOut");
		StartCoroutine(LoadGameAsync());
	}

	IEnumerator LoadGameAsync()
	{
		yield return new WaitForSeconds(1.5f);

		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(0);

		while (!asyncLoad.isDone)
		{
			yield return null;
		}
	}
}
