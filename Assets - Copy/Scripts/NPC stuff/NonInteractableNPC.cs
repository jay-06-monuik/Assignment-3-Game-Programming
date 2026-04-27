using UnityEngine;

public class NonInteractableNPCs : NPC
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Move();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    protected override void Move()
    {
        base.Move();
        Debug.Log("I am a non interactable move!");
    }
}
