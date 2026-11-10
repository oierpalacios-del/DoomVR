using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameObject xrOrigin;
    [SerializeField] private InputActionReference thumbstickPressAction;
    [SerializeField] private float speed;
    private float staminaCooldown = 1f;
    private float staminaCooldownTimer;
    [SerializeField] private bool pulsado;
    private ContinuousMoveProvider moveProvider;

    private void Start()
    {
        // Cachear el provider, no buscarlo cada frame
        moveProvider = xrOrigin.GetComponentInChildren<ContinuousMoveProvider>();
        if (moveProvider == null)
            Debug.LogWarning("No se encontró ContinuousMoveProvider en el XR Origin.");
    }

    private void OnEnable()
    {
        if (thumbstickPressAction == null) return;

        var action = thumbstickPressAction.action;
        action.Enable(); // importante en dispositivo
        action.started += OnPressed;
        action.performed += OnPressed;
        action.canceled += OnReleased;
    }

    private void OnDisable()
    {
        if (thumbstickPressAction == null) return;

        var action = thumbstickPressAction.action;
        action.started -= OnPressed;
        action.performed -= OnPressed;
        action.canceled -= OnReleased;
        pulsado = false;
    }

    private void OnPressed(InputAction.CallbackContext ctx)
    {
        pulsado = true;
        Debug.Log("Thumbstick pulsado");
    }

    private void OnReleased(InputAction.CallbackContext ctx)
    {
        pulsado = false;
        Debug.Log("Thumbstick soltado");
    }

    private void Update()
    {
        if (moveProvider == null) return;

        if (staminaCooldownTimer > 0)
            staminaCooldownTimer -= Time.deltaTime;

        speed = StatsManager.Instance.speed;
        bool enCooldown = staminaCooldownTimer > 0;

        if (StatsManager.Instance.stamina <= 0)
        {
            // Sin stamina: velocidad normal y arrancar cooldown
            moveProvider.moveSpeed = speed;
            staminaCooldownTimer = staminaCooldown;
            StatsManager.Instance.ChangeStamina(1);
        }
        else if (pulsado && !enCooldown)
        {
            // Corriendo
            moveProvider.moveSpeed = 2 * speed;
            StatsManager.Instance.ChangeStamina(-5);
        }
        else
        {
            // Caminando / recuperando
            moveProvider.moveSpeed = speed;
            StatsManager.Instance.ChangeStamina(1);
        }
    }
}
