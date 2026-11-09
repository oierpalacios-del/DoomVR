using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public bool dead;
    public void ChangeHealth(float amount)
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
