using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{ 
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float jumpVelocity = 10f;
    [SerializeField] private float moveSpeedAimed = 2f;
    [SerializeField] private float rotationSpeedAimed = 10f;
    [SerializeField] private Transform aimTrack;
    [SerializeField] private float maxAimHeight = 2f;
    [SerializeField] private float minAimHeight = -2f;
    [SerializeField] private Vector3 groundCheckOffset = new Vector3(0, 0.1f, 0);
    [SerializeField] private float groundCheckDistance = 0.3f;
    [SerializeField] private float groundCheckRadius = 0.3f;
    [SerializeField] private LayerMask groundLayer;

    public event Action OnJumpEvent;
    public event Action<PlayerState> OnStateUpdated;

    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private CharacterController _characterController;
    private Vector3 _velocity;
    private bool _isGrounded;
    private PlayerState _currentState = PlayerState.EXPLORE;
    private Vector3 _defaultAimTrackerPosition;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        if (aimTrack != null)
            _defaultAimTrackerPosition = aimTrack.localPosition;

        ChangeState(PlayerState.EXPLORE);
    }

    private void Update()
    {
        switch (_currentState)
        {
            case PlayerState.EXPLORE:
                CalculateMovementExplore();
                if (aimTrack != null)
                    aimTrack.localPosition = _defaultAimTrackerPosition;
                break;

            case PlayerState.AIM:
                CalculateMovementAim();
                UpdateAimTrack();
                break;
        }

        _characterController.Move(_velocity * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        CheckGrounded();

        if (_isGrounded && _velocity.y < 0)
            _velocity.y = -0.2f;
    }

    public void OnMove(InputValue value) => _moveInput = value.Get<Vector2>();
    public void OnLook(InputValue value) => _lookInput = value.Get<Vector2>();

    public void OnJump()
    {
        if (_isGrounded)
        {
            _velocity.y = jumpVelocity;
            OnJumpEvent?.Invoke();
        }
    }

    public void OnAim(InputValue value)
    {
        PlayerState newState = value.isPressed ? PlayerState.AIM : PlayerState.EXPLORE;
        ChangeState(newState);
    }

    private void ChangeState(PlayerState newState)
    {
        _currentState = newState;
        OnStateUpdated?.Invoke(_currentState);
    }

    private void CalculateMovementExplore()
    {
        Vector3 camForward = playerCamera.transform.forward;
        Vector3 camRight = playerCamera.transform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = camRight * _moveInput.x + camForward * _moveInput.y;

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        _velocity.x = moveSpeed * moveDirection.x;
        _velocity.z = moveSpeed * moveDirection.z;
        _velocity.y += gravity * Time.deltaTime;
    }

    private void CalculateMovementAim()
    {
        transform.Rotate(Vector3.up, _lookInput.x * rotationSpeedAimed * Time.deltaTime);

        Vector3 moveDirection = transform.right * _moveInput.x + transform.forward * _moveInput.y;

        _velocity = moveSpeedAimed * moveDirection;
        _velocity.y += gravity * Time.deltaTime;
    }

    private void UpdateAimTrack()
    {
        Vector3 pos = aimTrack.localPosition;
        pos.y += _lookInput.y * rotationSpeedAimed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minAimHeight, maxAimHeight);
        aimTrack.localPosition = pos;
    }

    private void CheckGrounded()
    {
        _isGrounded = Physics.SphereCast(
            transform.position + groundCheckOffset,
            groundCheckRadius,
            Vector3.down,
            out _,
            groundCheckDistance,
            groundLayer);
    }

    public bool IsGrounded() => _isGrounded;

    public Vector3 GetPlayerVelocity() => _velocity;

    void OnDrawGizmos()
    {
        Gizmos.color = _isGrounded ? Color.green : Color.red;
        Gizmos.DrawSphere(transform.position + groundCheckOffset, groundCheckRadius);
    }
}