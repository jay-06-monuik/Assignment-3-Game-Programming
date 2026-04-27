using UnityEngine;
using TMPro;

public class ChestCollector : MonoBehaviour
{ 
    [SerializeField] private TMP_Text chestCountText;
    [SerializeField] private int chestsToWin = 5;
    private int chestCount = 0;

    private void Start()
    {
        if (GameEvents.Instance == null)
        {
            Debug.LogError("GameEvents.Instance is still null in Start()!");
            return;
        }

        GameEvents.Instance.OnChestCollected += OnChestCollected;
        Debug.Log("Successfully subscribed to chest event from Start()");
    }

    private void OnDestroy()
    {
        if (GameEvents.Instance != null)
            GameEvents.Instance.OnChestCollected -= OnChestCollected;
    }

    public void CollectChest()
    {
        if (GameEvents.Instance != null)
            GameEvents.Instance.ChestCollected();
    }

    private void OnChestCollected()
    {
        chestCount++;
        if (chestCountText != null)
            chestCountText.text = $"Chests collected: {chestCount}";

        if (chestCount >= chestsToWin)
        {
            Debug.Log("=== YOU WIN THE GAME! ===");
        }
    }
    public int GetCurrentChestCount()
    {
        return chestCount;
    }
}