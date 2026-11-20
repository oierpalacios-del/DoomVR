
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class VRGun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform muzzle;
    public float bulletSpeed = 30f;

    private XRGrabInteractable grab;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
    }

    void OnEnable()
    {
        grab.activated.AddListener(Shoot);
    }

    void OnDisable()
    {
        grab.activated.RemoveListener(Shoot);
    }

    void Shoot(ActivateEventArgs args)
    {
        Debug.Log("¡DISPARO ACTIVADO!");

        if (bulletPrefab == null || muzzle == null)
            return;

        GameObject bullet = Instantiate(
            bulletPrefab,
            muzzle.position,
            muzzle.rotation
        );

        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
            rb.linearVelocity = muzzle.forward * bulletSpeed;

        Destroy(bullet, 5f);
    }

    public void Shoot()
    {
        Debug.Log("¡DISPARO ACTIVADO!");

        if (bulletPrefab == null || muzzle == null)
            return;

        GameObject bullet = Instantiate(
            bulletPrefab,
            muzzle.position,
            muzzle.rotation
        );

        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
            rb.linearVelocity = muzzle.forward * bulletSpeed;

        Destroy(bullet, 5f);
    }
}
