using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : CharacterControllerBase
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform feetPos;
    [SerializeField] private Transform bodyPos;
    [SerializeField] private float jumpForce;
    [SerializeField] private float groundDistance;
    [SerializeField] private float crouchDuration;
    private PlayerController playerController;
    private InputAction jumpAction;
    private InputAction crouchAction;
    private float crouchTimer = 0f;
    private float defaultCrouchHeight = 1f;
    private float crouchHeight = 0.5f;
    private float fallSpeed = 20f;
    private bool isCrouching = false;
    private void Awake()
    {
        playerController = new PlayerController();
    }

    protected override void Start()
    {
        base.Start();
        playerController.Player.Enable();
        PlayerInputActions();
    }

    protected override void Update()
    {
        if (isCrouching) {
            crouchTimer += Time.deltaTime;
            if (crouchTimer >= crouchDuration) {
                StandUp();
            }
        }
    }
    
    private void PlayerInputActions()
    {
        jumpAction = playerController.Player.Jump;
        jumpAction.performed += OnJumpPerformed;

        crouchAction = playerController.Player.Crouch;
        crouchAction.performed += OnCrouchPerformed;
    }


    private void OnJumpPerformed(InputAction.CallbackContext context) {
        Jump();
    }

    private void OnCrouchPerformed(InputAction.CallbackContext context)
    {
        Crouch();
    }

    protected override void Jump()
    {
        if(feetPos == null) return;

        if(IsGrounded()) {
            Rigidbody.linearVelocity = Vector2.up * jumpForce;
            StandUp();
        }
    }

    private void Crouch() {
        if (bodyPos == null) return;

        if (!IsGrounded()) {
            Rigidbody.linearVelocity = new Vector2(Rigidbody.linearVelocity.x, -fallSpeed);
        }

        bodyPos.localScale = new Vector3(bodyPos.localScale.x, crouchHeight, bodyPos.localScale.z);
        crouchTimer = 0f;
        isCrouching = true;
    }

    private void StandUp()
    {
        if(bodyPos == null) return;
        bodyPos.localScale = new Vector3(bodyPos.localScale.x, defaultCrouchHeight, bodyPos.localScale.z);
        isCrouching = false;
    }

    private void OnDisable()
    {
        jumpAction.performed -= OnJumpPerformed;
        crouchAction.performed -= OnCrouchPerformed;
        playerController.Player.Disable();
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(feetPos.position, groundDistance, groundLayer);
    }
}
