using UnityEngine;

public class EnemyBehaivor : MonoBehaviour
{
    public EnemyData enemyData;
    private Transform player;

    float attackTimer = 1f;
    float attackTimerCooldown;

    void Start()
    {
        attackTimerCooldown = attackTimer;
    }
    void Update()
    {
        if(attackTimerCooldown >= 0)
        {
            attackTimerCooldown -= Time.deltaTime;
        }
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player != null)
        {
            float dist = Vector3.Distance(transform.position, player.position);
            if (dist > enemyData.stopRange && dist < enemyData.detectRange)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position, player.position, enemyData.speed * Time.deltaTime);
                transform.LookAt(player);
            }
        }

        if(enemyData.vidaActual <= 0)
        {
            Destroy(gameObject);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("bullet"))
        {
            this.enemyData.vidaActual -= 1;
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (attackTimerCooldown <= 0)
        {
            attackTimerCooldown = attackTimer;
            if (other.gameObject.tag.Equals("Player"))
            {
                int damage = Random.Range(enemyData.damageMin, enemyData.damageMax);
                other.gameObject.GetComponent<PlayerHealth>().ChangeHealth(-damage);
            }
        }
    }
}