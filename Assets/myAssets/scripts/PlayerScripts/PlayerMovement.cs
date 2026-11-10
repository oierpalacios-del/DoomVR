using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameObject xrOrigin;
    [SerializeField] private InputActionReference thumbstickPressAction;
    [SerializeField] private InputActionReference moveAction; // NUEVO: la acción Vector2 del joystick de movimiento
    [SerializeField] private float moveThreshold = 0.1f;      // NUEVO: zona muerta para considerar que te mueves
    [SerializeField] private float speed;

    public float staminaCooldown = 1f;
    public float staminaCooldownTimer;
    [SerializeField] private bool pulsado;
    private bool agotado;
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
            moveAction.action.Enable();
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

    private void Update()
    {
        if (moveProvider == null) return;

        speed = StatsManager.Instance.speed;

        if (staminaCooldownTimer > 0)
            staminaCooldownTimer -= Time.deltaTime;

        float stamina = StatsManager.Instance.stamina;

        if (stamina <= 0)
            agotado = true;

        if (agotado && stamina >= 20f)
            agotado = false;

        // NUEVO: solo cuenta como "moviéndose" si el joystick supera la zona muerta
        bool moviendose = moveAction != null &&
                          moveAction.action.ReadValue<Vector2>().magnitude > moveThreshold;

        bool corriendo = pulsado && moviendose && !agotado && stamina > 0;

        moveProvider.moveSpeed = corriendo ? 2f * speed : speed;

        if (staminaCooldownTimer <= 0)
        {
            if (corriendo)
            {
                // NUEVO: nunca restar más de lo que queda, así no baja de 0
                float gasto = Mathf.Min(3f, stamina);
                StatsManager.Instance.ChangeStamina(-gasto);
                staminaCooldownTimer = staminaCooldown;
            }
            else if (stamina < StatsManager.Instance.staminalimit)
            {
                // NUEVO: nunca sumar más de lo que falta para el límite
                float recuperacion = Mathf.Min(2f, StatsManager.Instance.staminalimit - stamina);
                StatsManager.Instance.ChangeStamina(recuperacion);
                staminaCooldownTimer = staminaCooldown;
            }
        }
    }
}