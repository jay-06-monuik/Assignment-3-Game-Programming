using UnityEngine;
using UnityEngine.InputSystem;

public class Shooter : MonoBehaviour
{ 
        [SerializeField] private InputAction shootInput;
        [SerializeField] private Transform shootPoint;
        [SerializeField] private Transform aimTrack; 
        [SerializeField] private GameObject shootObject; 
        [SerializeField] private float shootForce = 20f; // added default value
        private PlayerController _playerController;
        private PlayerState _currentState;

        void Awake()
        {
                _playerController = GetComponent<PlayerController>();
        }

        void OnEnable()
        {
                shootInput.Enable();
                shootInput.performed += Shoot;
                _playerController.OnStateUpdated += HandleStateUpdate;
        }

        void OnDisable()
        {
                if (shootInput != null)
                        shootInput.performed -= Shoot;
        
                if (_playerController != null)
                        _playerController.OnStateUpdated -= HandleStateUpdate;
        }

        private void HandleStateUpdate(PlayerState state)
        {
                _currentState = state;
        }

        private void Shoot(InputAction.CallbackContext context)
        {
                if (_currentState != PlayerState.AIM) return;
        
                Vector3 direction = (aimTrack.position - shootPoint.position).normalized;
        
                GameObject arrow = Instantiate(shootObject, shootPoint.position, Quaternion.LookRotation(direction));
                arrow.GetComponent<Rigidbody>().AddForce(direction * shootForce, ForceMode.Impulse);
        }
}