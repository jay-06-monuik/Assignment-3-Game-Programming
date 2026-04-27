using UnityEngine;

public class WinManager : MonoBehaviour
{ 
    [SerializeField] private GameObject winMenu;

    private void Start()
    {
        Debug.Log("WinManager Start() ran successfully!");

        if (GameEvents.Instance != null)
        {
            GameEvents.Instance.OnChestCollected += CheckWinCondition;
            Debug.Log("WinManager successfully subscribed to event in Start()");
        }
        else
        {
            Debug.LogError("GameEvents.Instance was null in WinManager.Start()!");
        }
    }

    private void OnDestroy()
    {
        if (GameEvents.Instance != null)
            GameEvents.Instance.OnChestCollected -= CheckWinCondition;
    }
    private void CheckWinCondition()
    {
        Debug.Log($"WinManager received event. Current chest count = ?");

        ChestCollector collector = GetComponent<ChestCollector>();
    
        if (collector == null)
        {
            Debug.LogError("Could not find ChestCollector on this object!");
            return;
        }

        int current = collector.GetCurrentChestCount();
        Debug.Log($"Current chests collected: {current} / 5");

        if (current >= 4)
        {
            Debug.Log("=== WIN CONDITION MET! Showing Win Screen ===");
            winMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void ShowWinScreen()
    {
        if (winMenu != null)
        {
            winMenu.SetActive(true);
            Time.timeScale = 0f; 
        }
    }
}