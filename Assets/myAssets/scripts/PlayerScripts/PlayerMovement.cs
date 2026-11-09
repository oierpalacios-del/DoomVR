using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameObject xrOrigin;
    [SerializeField] bool pulsado;
    [SerializeField] private InputActionReference thumbstickPressAction;
    [SerializeField] private float speed;
    private float staminaCooldown = 1;
    private float staminaCooldownTimer;
    private void OnEnable()
    {
        if (thumbstickPressAction != null)
        {
            thumbstickPressAction.action.performed += OnThumbstickPressed;
            pulsado = true;
        }
    }

    private void OnDisable()
    {
        if (thumbstickPressAction != null)
        {
            thumbstickPressAction.action.performed -= OnThumbstickPressed;
            pulsado = false;
        }
    }

    private void OnThumbstickPressed(InputAction.CallbackContext context)
    {
        Debug.Log("Thumbstick pulsado");
    }

    private void Update()
    {
        if (staminaCooldownTimer > 0)
        {
            staminaCooldownTimer -= Time.deltaTime;
        }
        speed = StatsManager.Instance.speed;
        var moveProvider = xrOrigin.GetComponentInChildren<ContinuousMoveProvider>();

        if (moveProvider != null)
        {
            if(StatsManager.Instance.stamina <= 0 && staminaCooldownTimer > 0)
            {
                moveProvider.moveSpeed = speed;
                StatsManager.Instance.ChangeStamina(1);
                staminaCooldownTimer = staminaCooldown;
            }
            else if(StatsManager.Instance.stamina > 0 && pulsado && staminaCooldownTimer > 0)
            {
                moveProvider.moveSpeed = 2 * speed;
                StatsManager.Instance.ChangeStamina(-5);
            }
            else
            {
                StatsManager.Instance.ChangeStamina(1);
            }
        }
        else
        {
            Debug.LogWarning("No se encontró ContinuousMoveProvider en el XR Origin.");
        }
    }
}
