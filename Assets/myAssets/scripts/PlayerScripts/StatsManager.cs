using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance;
    public PlayerHealth player;
    [Header("Combat Stats")]
    public float damage;
    public float weaponRange;
    public float knockbackForce;
    public float knockbackTime;
    public float stunTime;
    //public int habilCooldown;
    //public float habilDamage;
    public float meeleDefense;
    [Header("Movement Stats")]
    public float speed;
    //public float ClimbSpeed;
    public float stamina;
    public float staminaSinItem;
    public float staminalimit;
    //public float jumpHeight;

    [Header("Health Stats")]
    public float maxHealth;
    public float currentHealth;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        //maxHealth = 120;
        //staminalimit = 120;
        //damage = 10;
        //weaponRange = 10;
        //knockbackForce = 2;
        //knockbackTime = 0.15f;
        //stunTime = 2;
        //meeleDefense = 0;
        //habilCooldown = 10;
        //habilDamage = 10;
        //currentHealth = maxHealth;
        //ClimbSpeed = 3;
        //speed = 12;
        //staminalimit = 120;
        //jumpHeight = 2;
    }
    public void ChangeStamina(float amount)
    {
        if (staminaSinItem < staminalimit)
        {
            staminaSinItem += amount;
            UpdateStamina(amount);
        }
        else
        {
            staminaSinItem = staminalimit;
        }
    }
    //public void UpdateJumpForce(float amount)
    //{
        //jumpHeight += amount;
    //}
    public void UpdateCurrentHealth(float amount)
    {
        player.ChangeHealth(amount);
    }
    public void UpdateMaxHealth(float amount)
    {
        maxHealth += amount;
    }
    public void UpdateSpeed(float amount)
    {
        speed += amount;
    }
    public void UpdateStamina(float amount)
    {
        stamina += amount;
    }
    public void UpdateFisicDefense(float amount)
    {
        meeleDefense += amount;
    }
    public void UpdateMeeleDamage(float amount)
    {
        damage += amount;
    }
}
