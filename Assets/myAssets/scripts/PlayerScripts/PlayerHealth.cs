using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public bool dead;
    [SerializeField] private UIUpdater uiUpdater;
    public void ChangeHealth(int amount)
    {
        StatsManager.Instance.currentHealth += amount;
        if (StatsManager.Instance.currentHealth >= StatsManager.Instance.maxHealth)
        {
            StatsManager.Instance.currentHealth = StatsManager.Instance.maxHealth;
        }
        if (StatsManager.Instance.currentHealth <= 0)
        {
            dead = true;
            gameObject.SetActive(false);
        }
        uiUpdater.UpdateVida(StatsManager.Instance.currentHealth, StatsManager.Instance.maxHealth);
    }
    public bool IsDead()
    {
        return dead;
    }
    void Start()
    {
        dead = false;
    }
}
