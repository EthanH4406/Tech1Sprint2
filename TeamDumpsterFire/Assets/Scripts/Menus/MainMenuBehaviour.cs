using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuBehaviour : MonoBehaviour
{
    public CanvasGroup OptionPanel;
    public Animator animator;

    public void PlayGame()
    {
        animator.SetTrigger("FadeOut");

        StartCoroutine(LoadGameAsync());

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);   
    }

    public void Option()
    {
        OptionPanel.alpha = 1;
        OptionPanel.blocksRaycasts = true;
    }

	public void Back()
	{
		OptionPanel.alpha = 0;
		OptionPanel.blocksRaycasts = false;
	}

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadGameAsync()
    {
		yield return new WaitForSeconds(1.5f);

		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        while(!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
