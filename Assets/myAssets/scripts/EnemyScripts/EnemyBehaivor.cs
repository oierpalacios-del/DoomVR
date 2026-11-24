using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaivor : MonoBehaviour
{
    public EnemyData enemyData;
    private Transform player;

    [SerializeField] private NavMeshAgent agent;
    
    float attackTimer = 1f;
    float attackTimerCooldown;
    float dist;

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
        dist = Vector3.Distance(transform.position, player.position);
        if (enemyData.vuela)
        {
            if (player != null)
            {
                if (dist > enemyData.stopRange && dist < enemyData.detectRange)
                {
                    transform.position = Vector3.MoveTowards(
                        transform.position, player.position, enemyData.speed * Time.deltaTime);
                    transform.LookAt(player);
                }
            }
        }
        else
        {
            if (player != null && dist > enemyData.stopRange && dist < enemyData.detectRange)
            {
                agent.SetDestination(player.position);
            }
        }
    }
    public void TakeDamage(int amount)
    {
        enemyData.vidaActual += amount;
        if (enemyData.vidaActual <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("bullet"))
        {
            TakeDamage(-1);
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