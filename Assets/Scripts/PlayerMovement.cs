using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : CharacterController
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform feetPos;
    [SerializeField] private float jumpForce;
    [SerializeField] private float groundDistance;
    private PlayerController playerController;
    private InputAction jumpAction;

    private void Awake()
    {
        playerController = new PlayerController();
    }

    protected override void Start()
    {
        base.Start();
        playerController.Player.Enable();

        jumpAction = playerController.Player.Jump;
        jumpAction.performed += OnJumpPerformed;
    }

    private void OnJumpPerformed(InputAction.CallbackContext context) {
        Jump();
    }

    protected override void Jump()
    {
        if(feetPos == null) return;

        if(IsGrounded()) {
            Rigidbody.linearVelocity = Vector2.up * jumpForce;
        }
    }

    private void OnDisable()
    {
        jumpAction.performed -= OnJumpPerformed;
        playerController.Player.Disable();
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(feetPos.position, groundDistance, groundLayer);
    }
}
