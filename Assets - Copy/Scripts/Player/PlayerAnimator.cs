using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerController playerController; 
    [SerializeField] private Animator anim;

    void Update()
    {
        anim.SetBool("IsGrounded",playerController.IsGrounded());

        Vector3 velocity = playerController.GetPlayerVelocity();
        velocity.y = 0;
        anim.SetFloat("Velocity", velocity.sqrMagnitude);
    }

    void OnEnable()
    {
        playerController.OnJumpEvent += OnJump;
    }

    void OnDisable()
    {
        playerController.OnJumpEvent -= OnJump;
    }

    private void OnJump()
    {
        anim.SetTrigger("Jump");
    }
}