using UnityEngine;
using TMPro;
public class UIUpdater : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text municionText;
    [SerializeField] private TMP_Text granadasText;
    [SerializeField] private TMP_Text staminaText;
    public void UpdateStamina(int stamina, int limit)
    {
        staminaText.text = "Energía:    " + stamina + " / " + limit;
    }
    public void UpdateVida(int vida, int max)
    {
        healthText.text = "Vida:    " + vida + " / " + max;
    }
}
