using UnityEngine;

public class GameOverManager : MonoBehaviour
{ 
    [SerializeField] private GameObject gameOverMenu;

    private void Start()
    {
        if (GameEvents.Instance != null)
            GameEvents.Instance.OnPlayerDied += ShowGameOver;
    }

    private void OnDestroy()
    {
        if (GameEvents.Instance != null)
            GameEvents.Instance.OnPlayerDied -= ShowGameOver;
    }

    private void ShowGameOver()
    {
        if (gameOverMenu != null)
        {
            gameOverMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void SetGameOverMenu(GameObject menu)
    {
        gameOverMenu = menu;
    }
}