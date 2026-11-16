using UnityEngine;
using UnityEngine.XR;

public class FollowLeftController : MonoBehaviour
{
    [SerializeField] private Transform leftController;
    [SerializeField] private Quaternion rotationOffset = Quaternion.Euler(0, -90, 0);
    [SerializeField] private Vector3 positionOffset = Vector3.zero;

    private Collider[] colliders;
    private Renderer[] renderers;
    private bool isActive = false;

    private void Awake()
    {
        colliders = GetComponentsInChildren<Collider>();
        renderers = GetComponentsInChildren<Renderer>();
        SetShieldActive(false);
    }

    private void LateUpdate()
    {
        if (leftController == null) return;

        // Copiar posición y rotación
        transform.position = leftController.position + leftController.rotation * positionOffset;
        transform.rotation = leftController.rotation * rotationOffset;

        // Detectar trigger del controlador izquierdo
        InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        bool triggerPressed = false;

        if (device.isValid)
        {
            device.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
            triggerPressed = triggerValue > 0.1f; // Umbral para evitar ruido
        }

        // Activar/desactivar solo si cambia el estado
        if (triggerPressed != isActive)
        {
            isActive = triggerPressed;
            SetShieldActive(isActive);
        }
    }

    private void SetShieldActive(bool active)
    {
        foreach (var c in colliders)
            c.enabled = active;

        foreach (var r in renderers)
            r.enabled = active;
    }
}