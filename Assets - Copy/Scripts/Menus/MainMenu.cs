using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Game");     // Make sure "Game" matches your game scene name
    }

    public void QuitGame()
    {
        Debug.Log("Quitting from Main Menu");
        Application.Quit();
    }
}