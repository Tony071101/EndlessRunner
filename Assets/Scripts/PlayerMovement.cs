using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class PlayerMovement : CharacterControllerBase
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform feetPos;
    private Transform bodyPos;
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
    private float swipeMinDistance = 50f; //pixels.
    private Vector2 swipeStartPos;
    private bool isCrouching = false;
    private bool isRunning;
    private bool hasSwiped = false;

    private void Awake()
    {
        playerController = new PlayerController();
    }

    protected override void Start()
    {
        base.Start();
        playerController.Player.Enable();
        PlayerInputActions();
        GameManager.Instance.onPlay.AddListener(OnGameStarted);
        GameManager.Instance.onGameOver.AddListener(OnGameOver);
        EnhancedTouchSupport.Enable();
        GameObject bodyObj = GameObject.FindGameObjectWithTag("Body");
        if (bodyObj != null)
        {
            bodyPos = bodyObj.transform;
        }
    }

    protected override void Update()
    {
        if (isCrouching) {
            crouchTimer += Time.deltaTime;
            if (crouchTimer >= crouchDuration) {
                StandUp();
            }
        }

        PlayerAnims();
        DetectSwipe();
    }
    
    private void PlayerInputActions()
    {
        jumpAction = playerController.Player.Jump;
        jumpAction.performed += context => {
            if (GameManager.Instance.isPlaying)
                OnJumpPerformed(context);
        };


        crouchAction = playerController.Player.Crouch;
        crouchAction.performed += context => {
            if (GameManager.Instance.isPlaying)
                OnCrouchPerformed(context);
        };
    }


    private void OnJumpPerformed(InputAction.CallbackContext context) {
        Jump();
    }

    private void OnCrouchPerformed(InputAction.CallbackContext context)
    {
        Crouch();
    }

    private void OnGameStarted()
    {
        isRunning = true;
    }

    private void OnGameOver()
    {
        isRunning = false;
    }

    protected override void Jump()
    {
        if(feetPos == null) return;

        if(IsGrounded()) {
            Rigidbody.linearVelocity = Vector2.up * jumpForce;
            StandUp();
            _anim?.SetTrigger("Jump");
        }
    }

    private void Crouch() {
        if (bodyPos == null) return;

        if (!IsGrounded()) {
            Rigidbody.linearVelocity = new Vector2(Rigidbody.linearVelocity.x, -fallSpeed);
        }

        bodyPos.localScale = new Vector3(bodyPos.localScale.x, crouchHeight, bodyPos.localScale.z);
        _anim?.SetTrigger("Crouch");
        crouchTimer = 0f;
        isCrouching = true;
    }

    private void StandUp()
    {
        if(bodyPos == null) return;
        bodyPos.localScale = new Vector3(bodyPos.localScale.x, defaultCrouchHeight, bodyPos.localScale.z);
        isCrouching = false;
    }


    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(feetPos.position, groundDistance, groundLayer);
    }

    private void DetectSwipe() {
        if (!GameManager.Instance.isPlaying) return;
        if (Touch.activeTouches.Count == 0) return;
        foreach(var touch in Touch.activeTouches) {
            switch (touch.phase)
            {
                case UnityEngine.InputSystem.TouchPhase.Began:
                    swipeStartPos = touch.screenPosition;
                    hasSwiped = false;
                    break;

                case UnityEngine.InputSystem.TouchPhase.Moved:
                    if (hasSwiped) return;

                    Vector2 swipeDelta = touch.screenPosition - swipeStartPos;

                    if (swipeDelta.magnitude > swipeMinDistance)
                    {
                        float verticalRatio = Mathf.Abs(swipeDelta.y) / swipeDelta.magnitude;

                        if (verticalRatio > 0.7f)
                        {
                            if (swipeDelta.y > 0)
                            {
                                Jump();
                                hasSwiped = true;
                            }
                            else
                            {
                                Crouch();
                                hasSwiped = true;
                            }
                        }
                    }
                    break;

                case UnityEngine.InputSystem.TouchPhase.Ended:
                case UnityEngine.InputSystem.TouchPhase.Canceled:
                    hasSwiped = false;
                    break;
            }
        }
    }

    private void PlayerAnims() {
        if(_anim == null) return;

        if (!IsGrounded() && Rigidbody.linearVelocity.y < 0)
        {
            _anim?.SetBool("IsFalling", true);
        }
        else
        {
            _anim?.SetBool("IsFalling", false);
            _anim?.SetBool("IsRunning", isRunning);
        }
    }

    public void RefreshCharacterReferences()
    {
        _anim = GetComponentInChildren<Animator>();
        GameObject bodyObj = GameObject.FindGameObjectWithTag("Body");
        if (bodyObj != null)
        {
            bodyPos = bodyObj.transform;
        }
        else
        {
            Debug.LogWarning("Could not find Body (tagged) under Player.");
        }
    }

    // private void OnDisable()
    // {
    //     jumpAction.performed -= OnJumpPerformed;
    //     crouchAction.performed -= OnCrouchPerformed;
    //     playerController.Player.Disable();
    // }

    // private void OnDestroy()
    // {
    //     if (GameManager.Instance != null)
    //     {
    //         GameManager.Instance.onPlay.RemoveListener(OnGameStarted);
    //         GameManager.Instance.onGameOver.RemoveListener(OnGameOver);
    //     }

    //     EnhancedTouchSupport.Disable(); // Tắt khi không cần
    // }
}
