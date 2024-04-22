using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform[] laneTransforms;
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] [Range(0,1)] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundCheckMask;
    [SerializeField] private Vector3 blockageCheckHalfExtend;
    [SerializeField] private string blockageCheckTag = "Obstacle";
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private float jumpHeight = 2.5f;
    [SerializeField] private InGameUI inGameUI;

    [Header("Auido")]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip moveSound;
    [SerializeField] private AudioSource actionAudioSource;

    [Header("Particle System")]
    [SerializeField] private ParticleSystem speedParticles;
    [SerializeField] private Transform hitParticlesSpawnPoint;
    [SerializeField] private GameObject hitParticles;

    private Rigidbody rb;
    private PlayerInput playerInput;
    private Vector3 destination;
    private int currentLaneIdx = 0;
    private Camera mainCamera;
    private Vector3 cameraOffset;

    private Animator animator;
    private SpeedController speedController;

    private void Awake()
    {
        playerInput ??= new PlayerInput();
        playerInput.Gameplay.Enable();
        playerInput.Gameplay.Move.performed += MovePerformed;
        playerInput.Gameplay.Jump.performed += JumpPerformed;
        
        playerInput.Menu.Enable();
        playerInput.Menu.Pause.performed += TogglePause;
    }


    private void Start()
    {
        CalculatePlayerPosition();

        rb = GetComponent<Rigidbody>();

        animator = GetComponent<Animator>();

        mainCamera = Camera.main;
        cameraOffset = mainCamera.transform.position - transform.position;

        speedController = Utils.GetGameManager().GetComponent<SpeedController>();
        speedController.OnSpeedBoostActivated += OnSpeedBoostActivated;
        speedController.OnSpeedBoostDeactivated += OnSpeedBoostDeactivated;
        speedParticles = GetComponentInChildren<ParticleSystem>();
    }


    private void OnSpeedBoostActivated()
    {
        if (!speedParticles.isPlaying)
        {
            speedParticles.Play();
        }
    }


    private void OnSpeedBoostDeactivated()
    {
        if (speedParticles.isPlaying)
        {
            speedParticles.Stop();
        }
    }


    private void Update()
    {
        if (!IsOnGround())
        {
            animator.SetBool("isOnGround", false);
        }
        else
        {
            animator.SetBool("isOnGround", true);
        }

        //transform.position = Vector3.Lerp(transform.position, destination, moveSpeed * Time.deltaTime);
        float transformX = Mathf.Lerp(transform.position.x, destination.x, moveSpeed * Time.deltaTime);
        transform.position = new Vector3(transformX, transform.position.y, transform.position.z);
    }


    private void FixedUpdate()
    {
        animator.SetFloat("yVelocity", rb.velocity.y);
    }


    private void LateUpdate()
    {
        mainCamera.transform.position = transform.position + cameraOffset;
    }


    private void JumpPerformed(InputAction.CallbackContext context)
    {
        if (!IsOnGround()) return; 

        if (rb != null)
        {
            float jumpSpeed = Mathf.Sqrt(2 * jumpHeight * Physics.gravity.magnitude);
            rb.AddForce(Vector3.up * jumpSpeed, ForceMode.VelocityChange);
            // rigidbody.velocity = new Vector3(0, jumpSpeed, 0);
            actionAudioSource.clip = jumpSound;
            actionAudioSource.Play();
        }
    }


    private void MovePerformed(InputAction.CallbackContext context)
    {
        // if (!IsOnGround()) return;
        float inputValue = context.ReadValue<float>();

        int goalIdx = currentLaneIdx;

        if (inputValue  > 0f) 
        {
            if (goalIdx == laneTransforms.Length - 1) return;
            goalIdx++;
        }
        else
        {
            if (goalIdx == 0) return;
            goalIdx--;
        }

        // Vector3 goalPosition = laneTransforms[goalIdx].position;
        Vector3 goalPosition = laneTransforms[goalIdx].position + new Vector3(0, groundCheckTransform.position.y + 0.1f, 0);
        if (Utils.IsPositionOccupied(goalPosition, blockageCheckHalfExtend, blockageCheckTag))
        {
            return;
        }

        actionAudioSource.clip = moveSound;
        actionAudioSource.Play();

        currentLaneIdx = goalIdx;
        destination = goalPosition;
    }


    private void TogglePause(InputAction.CallbackContext context)
    {
        GameManager gameManager = Utils.GetGameManager();
        if (gameManager != null && !gameManager.IsGameOver())
        {
            gameManager.TogglePause();
            inGameUI.SignalPause(gameManager.IsGamePaused());
        }
    }


    private void CalculatePlayerPosition()
    {
        for (int i = 0; i < laneTransforms.Length; i++)
        {
            if (laneTransforms[i].position == transform.position)
            {
                currentLaneIdx = i;
                destination = laneTransforms[i].position;
            }
        }
    }


    private void MoveLeft()
    {
        if (currentLaneIdx == 0) return;

        currentLaneIdx--;
        destination = laneTransforms[currentLaneIdx].transform.position;
    }


    private void MoveRight() 
    {
        if (currentLaneIdx == laneTransforms.Length - 1) return;

        currentLaneIdx++;
        destination = laneTransforms[currentLaneIdx].transform.position;
    }


    private bool IsOnGround()
    {
        return Physics.CheckSphere(groundCheckTransform.position, groundCheckRadius, groundCheckMask);
    }


    public void DisableGameplayInput()
    {
        playerInput.Gameplay.Disable();
    }


    public void EnableGameplayInput()
    {
        playerInput.Gameplay.Enable();
    }


    public void PlayHitEffect()
    {
        Instantiate(hitParticles, hitParticlesSpawnPoint.position, Quaternion.identity);
    }


    private void OnDisable()
    {
        playerInput.Disable();
        speedController.OnSpeedBoostActivated -= OnSpeedBoostActivated;
        speedController.OnSpeedBoostDeactivated -= OnSpeedBoostDeactivated;
    }
}
