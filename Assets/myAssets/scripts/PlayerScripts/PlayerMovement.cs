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
    [SerializeField] private float speed;

    // --- Sprint ---
    public float staminaCooldown = 1f;
    public float staminaCooldownTimer;
    [SerializeField] private bool pulsado;
    private bool agotado;

    // --- Jump ---
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask groundLayer;
    private float _verticalVelocity;
    //private bool _isGrounded;

    private ContinuousMoveProvider moveProvider;

    private void Start()
    {
        moveProvider = xrOrigin.GetComponentInChildren<ContinuousMoveProvider>();
        if (moveProvider == null)
            Debug.LogWarning("No se encontró ContinuousMoveProvider en el XR Origin.");

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
            moveAction.action.Enable();

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
            jumpAction.action.performed -= OnJumpPerformed;

        pulsado = false;
    }

    private void OnPressed(InputAction.CallbackContext ctx) => pulsado = true;
    private void OnReleased(InputAction.CallbackContext ctx) => pulsado = false;

    // --- Jump ---
    private void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        if (controller.isGrounded)
            _verticalVelocity = jumpSpeed;
    }

    private bool IsGrounded()
    {
        // Raycast desde el XR Origin hacia abajo
        return Physics.Raycast(
            xrOrigin.transform.position,
            Vector3.down,
            groundCheckDistance,
            groundLayer
        );
    }

    private void Update()
    {
        if (moveProvider == null) return;

        speed = StatsManager.Instance.speed;

        // --- Sprint (sin cambios) ---
        if (staminaCooldownTimer > 0)
            staminaCooldownTimer -= Time.deltaTime;

        float stamina = StatsManager.Instance.stamina;

        if (stamina <= 0) agotado = true;
        if (agotado && stamina >= 20f) agotado = false;

        bool moviendose = moveAction != null &&
                          moveAction.action.ReadValue<Vector2>().magnitude > moveThreshold;
        bool corriendo = pulsado && moviendose && !agotado && stamina > 0;

        moveProvider.moveSpeed = corriendo ? 2f * speed : speed;

        if (staminaCooldownTimer <= 0)
        {
            if (corriendo)
            {
                float gasto = Mathf.Min(3f, stamina);
                StatsManager.Instance.ChangeStamina(-gasto);
                staminaCooldownTimer = staminaCooldown;
            }
            else if (stamina < StatsManager.Instance.staminalimit)
            {
                float recuperacion = Mathf.Min(2f, StatsManager.Instance.staminalimit - stamina);
                StatsManager.Instance.ChangeStamina(recuperacion);
                staminaCooldownTimer = staminaCooldown;
            }
        }

        // --- Jump ---
        //_isGrounded = IsGrounded();

        // Gravedad
        if (!controller.isGrounded)
            _verticalVelocity += Physics.gravity.y * Time.deltaTime;
        else
            _verticalVelocity = 0f;

        if (Mathf.Abs(_verticalVelocity) > 0.001f)
        {
            controller.Move(Vector3.up * _verticalVelocity * Time.deltaTime);
        }
    }
}   