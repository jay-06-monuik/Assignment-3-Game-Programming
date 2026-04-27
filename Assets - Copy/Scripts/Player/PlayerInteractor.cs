using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{ 
    
    [SerializeField] private InputAction interactionInput;

    private IInteractable _interactable;

    void OnEnable()
    {
        interactionInput.Enable();
        interactionInput.performed += Interact;
    }

    void OnDisable()
    {
        if (interactionInput != null)
            interactionInput.performed -= Interact;

        interactionInput.Disable();
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        
        if (interactable != null)
        {
            _interactable = interactable;
            _interactable.OnHoverIn();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();

        if (interactable != null && interactable == _interactable)
        {
            _interactable.OnHoverOff();
            _interactable = null;
        }
    }

    private void Interact(InputAction.CallbackContext context)
    {
        _interactable?.OnInteract();
    }
}