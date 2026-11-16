using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameObject xrOrigin;
    [SerializeField] private InputActionReference thumbstickPressAction;
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction; // NUEVO
    [SerializeField] private CharacterController controller;
    [SerializeField] private float moveThreshold = 0.1f;
    [SerializeField] private UIUpdater uiUpdater;

    // --- Sprint ---
    public float staminaCooldown = 1f;
    public float staminaCooldownTimer;
    [SerializeField] private bool pulsado;
    private bool agotado;
    public bool corriendo;

    // --- Jump ---
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask groundLayer;
    private float verticalVelocity;

    private ContinuousMoveProvider moveProvider;

    private void Start()
    {
        moveProvider = xrOrigin.GetComponentInChildren<ContinuousMoveProvider>();
        if (moveProvider == null)
        {
            Debug.LogWarning("No se encontró ContinuousMoveProvider en el XR Origin.");
        }

        pulsado = false;
        agotado = false;
        staminaCooldownTimer = staminaCooldown;
    }

    private void OnEnable()
    {
        if (thumbstickPressAction != null)
        {
            var action = thumbstickPressAction.action;
            action.Enable();
            action.started += OnPressed;
            action.performed += OnPressed;
            action.canceled += OnReleased;
        }

        if (moveAction != null)
        {
            moveAction.action.Enable();
        }

        if (jumpAction != null)
        {
            jumpAction.action.Enable();
            jumpAction.action.performed += OnJumpPerformed;
        }
    }

    private void OnDisable()
    {
        if (thumbstickPressAction != null)
        {
            var action = thumbstickPressAction.action;
            action.started -= OnPressed;
            action.performed -= OnPressed;
            action.canceled -= OnReleased;
        }

        if (jumpAction != null)
        {
            jumpAction.action.performed -= OnJumpPerformed;
        }

        pulsado = false;
    }

    private void OnPressed(InputAction.CallbackContext ctx)
    {
        pulsado = true;
    }
    private void OnReleased(InputAction.CallbackContext ctx)
    {
        pulsado = false;
    }

    // --- Jump ---
    private void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        if (controller.isGrounded && StatsManager.Instance.stamina >= 5)
        {
            verticalVelocity = StatsManager.Instance.jumpSpeed;
            StatsManager.Instance.ChangeStamina(-5);
        } 
    }

    private void Update()
    {
        if (moveProvider == null)
        {
            return;
        }

        // --- Sprint (sin cambios) ---
        if (staminaCooldownTimer > 0)
        {
            staminaCooldownTimer -= Time.deltaTime;
        }


        if (StatsManager.Instance.stamina <= 0)
        {
            agotado = true;
        }
        if (agotado && StatsManager.Instance.stamina >= 20f)
        {
            agotado = false;
        }

        bool moviendose = moveAction != null && moveAction.action.ReadValue<Vector2>().magnitude > moveThreshold;
        corriendo = pulsado && moviendose && !agotado && StatsManager.Instance.stamina > 0;

        moveProvider.moveSpeed = corriendo ? 2f * StatsManager.Instance.speed : StatsManager.Instance.speed;

        if (staminaCooldownTimer <= 0)
        {
            if (corriendo)
            {
                StatsManager.Instance.ChangeStamina(-3);
                staminaCooldownTimer = staminaCooldown;
            }
            else if (StatsManager.Instance.stamina < StatsManager.Instance.staminalimit)
            {
                StatsManager.Instance.ChangeStamina(1);
                staminaCooldownTimer = staminaCooldown;
            }
        }

        // --- Jump ---

        // Gravedad
        if (!controller.isGrounded)
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            verticalVelocity = 0f;
        }

        if (Mathf.Abs(verticalVelocity) > 0.001f)
        {
            controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);
        }
        uiUpdater.UpdateStamina(StatsManager.Instance.stamina, StatsManager.Instance.staminalimit);
    }
}   