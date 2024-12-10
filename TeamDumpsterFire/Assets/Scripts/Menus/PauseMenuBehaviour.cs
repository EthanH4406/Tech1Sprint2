using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuBehaviour : MonoBehaviour
{
    public bool isPaused;

    public CanvasGroup optionsPanel;
    public CanvasGroup gameUI;
    public CanvasGroup Menu;

    private void DisableOptions()
    {
        optionsPanel.alpha = 0;
        optionsPanel.blocksRaycasts = false;
    }

    private void EnableOptions()
    {
        optionsPanel.alpha = 1;
        optionsPanel.blocksRaycasts = true;
    }

    private void DisableGame()
    {
        gameUI.alpha = 0;
        gameUI.blocksRaycasts = false;
    }

    private void EnableGame()
    {
        gameUI.alpha = 1;
        gameUI.blocksRaycasts = true;
    }

    private void DisableMenu()
    {
        Menu.alpha = 0;
        Menu.blocksRaycasts = false;
    }

    private void EnableMenu()
    {
        Menu.alpha = 1;
        Menu.blocksRaycasts = true;
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;

        DisableGame();
        EnableMenu();
    }

    public void UnPauseGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

		DisableMenu();
        EnableGame();
	}

    public void OpenOptionsMenu()
    {
        DisableMenu();
        EnableOptions();
    }

    public void BackToGame()
    {
        //close the menu
        UnPauseGame();
    }

    public void Back()
    {
        //exit out of the options menu
        DisableOptions();
        EnableMenu();
    }

    public void ReturnToMenu()
    {
        //return to main menu
        //SceneManager.LoadScene(SceneManager.GetSceneByBuildIndex(0));
    }
}
