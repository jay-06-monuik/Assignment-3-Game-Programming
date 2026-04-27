using UnityEngine;
using TMPro;

public class Toast : MonoBehaviour
{
    public static Toast Instance
    {
        get; private set;
        
    } 
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text toastText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        // Safety check - warn if references are missing
        if (panel == null)
            Debug.LogWarning("Toast: Panel reference is missing!");
        if (toastText == null)
            Debug.LogWarning("Toast: Text reference is missing!");

        if (panel != null)
            panel.SetActive(false);
    }

    public void ShowToast(string message)
    {
        if (toastText != null)
            toastText.text = message;
        
        if (panel != null)
            panel.SetActive(true);
    }

    public void HideToast()
    {
        if (panel != null)
            panel.SetActive(false);
    }
}