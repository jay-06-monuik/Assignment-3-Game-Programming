using UnityEngine;

public class PlayerHealth : MonoBehaviour
{ 
    [SerializeField] private int maxHealth = 100; 
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log($"Player took {damageAmount} damage. Health left: {currentHealth}");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        GameEvents.Instance.PlayerDied();
        
        // Disable player movement
        PlayerController playerController = GetComponent<PlayerController>();
        if (playerController != null)
            playerController.enabled = false;
    }

    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
}