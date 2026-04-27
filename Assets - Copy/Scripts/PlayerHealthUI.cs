using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{ 
    private Slider healthSlider;
    private PlayerHealth playerHealth;

    private void Start()
    {
        playerHealth = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
        healthSlider = GetComponentInChildren<Slider>();
        
        if (playerHealth != null && healthSlider != null)
            healthSlider.maxValue = playerHealth.GetMaxHealth();
    }

    private void Update()
    {
        if (playerHealth != null && healthSlider != null)
            healthSlider.value = playerHealth.GetCurrentHealth();
    }
}